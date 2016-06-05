namespace Auctionata.Api.Controllers
{
	using System.Linq;
	using Amazon.DynamoDBv2;
	using Amazon.DynamoDBv2.DataModel;
	using Amazon.DynamoDBv2.DocumentModel;
	using Amazon.SQS;
	using Amazon.SQS.Model;
	using Newtonsoft.Json;
	using System.Collections.Generic;
	using System.Web.Http;
	using Auctionata.Api.Models;

	public class AuctionController : ApiController
	{
		public IEnumerable<Item> Get(string code, bool onlyActive)
		{
			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				if (onlyActive)
				{
					return new List<Item>
					{
						context.Scan<Item>(
							new ScanCondition("AuctionCode", ScanOperator.BeginsWith, code),
							new ScanCondition("Status", ScanOperator.Equal, "Active"))
							.FirstOrDefault()
					};
				}

				return context.Scan<Item>(new ScanCondition("AuctionCode", ScanOperator.BeginsWith, code)).OrderBy(i => i.Id).ToList();
			}

		}

		public void Post([FromBody] Bid bid)
		{
			if (bid?.User != null && bid.Amount > 0 && bid.ItemId > 0 && bid.AuctionCode != null)
			{
				using (var sqs = new AmazonSQSClient())
				{
					string bidQueueUrl = FindBidQueueUrl(bid.AuctionCode, sqs);
					if (bidQueueUrl != null)
					{
						var sendMessageRequest = new SendMessageRequest { QueueUrl = bidQueueUrl, MessageBody = JsonConvert.SerializeObject(bid) };
						sqs.SendMessage(sendMessageRequest);
					}
				}
			}
		}

		private static string FindBidQueueUrl(string auctionCode, AmazonSQSClient sqs)
		{
			var listQueuesRequest = new ListQueuesRequest();
			var listQueuesResponse = sqs.ListQueues(listQueuesRequest);
			return listQueuesResponse.QueueUrls?.FirstOrDefault(queueUrl => queueUrl.EndsWith(auctionCode + "_Bid"));
		}
	}
}
