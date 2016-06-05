namespace Auctionata.Runner
{
	using System.Globalization;
	using System.Linq;
	using System.Timers;
	using Amazon.DynamoDBv2;
	using Amazon.DynamoDBv2.DataModel;
	using Amazon.DynamoDBv2.DocumentModel;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using Amazon.SQS;
	using Amazon.SQS.Model;

	internal class Program
	{
		private static string bidQueueUrl;
		private static readonly Timer Timer = new Timer(1000);
		private static int minutesToWaitForBid = 1;
		private static string auctionCode;

		public static void Main(string[] args)
		{
			auctionCode = args[0];
			bidQueueUrl = CreateBidQueue();

			Timer.Elapsed += TimerElapsed;
			Timer.Start();

			Console.WriteLine("Press Enter to continue...");
			Console.Read();
		}

		private static void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			Timer.Stop();

			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				var allItems = context.Scan<Item>(new ScanCondition("AuctionCode", ScanOperator.BeginsWith, auctionCode)).OrderBy(i => i.Id);

				if (AuctionFinished(allItems))
				{
					return;
				}

				if (AuctionHasNotStarted(allItems))
				{
					StartAuction(allItems, context);
				}
				else
				{
					var bidsToProcess = ReadAllBidsFromQueue();
					var currentItem = allItems.FirstOrDefault(i => i.Status == "Active");
					if (bidsToProcess.Any())
					{
						ProcessBids(currentItem, bidsToProcess, context);
					}
					else
					{
						if (EnoughTimeHasPassedSinceLastBid(currentItem, context))
						{
							FinishListeningToBidForCurrentAuctionedItem(currentItem, context);
						}
					}
				}
			}

			Timer.Start();
		}

		private static void FinishListeningToBidForCurrentAuctionedItem(Item currentItem, DynamoDBContext context)
		{
			currentItem.Status = "Finished";
			context.Save(currentItem);
		}

		private static bool EnoughTimeHasPassedSinceLastBid(Item currentItem, DynamoDBContext context)
		{
			DateTime lastTimestamp;
			if (DateTime.TryParse(currentItem.Timestamp, CultureInfo.InvariantCulture, DateTimeStyles.None, out lastTimestamp))
			{
				if (DateTime.Now.Subtract(lastTimestamp).Minutes > minutesToWaitForBid)
				{
					return true;
				}
			}
			else
			{
				currentItem.Timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
				context.Save(currentItem);
			}
			return false;
		}

		private static void ProcessBids(Item currentItem, List<Bid> bidsToProcess, DynamoDBContext context)
		{
			foreach (var bid in bidsToProcess)
			{
				if (bid != null
					&& bid.ItemId == currentItem.Id
					&& bid.Amount >= currentItem.HighestBid
					&& bid.Amount >= currentItem.StartingPrice)
				{
					currentItem.HighestBid = bid.Amount;
					currentItem.User = bid.User;
					currentItem.Timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
					currentItem.AskingPrice = BidIncrementLogic.IncrementBid(currentItem.AskingPrice);
					context.Save(currentItem);
				}
			}
		}

		private static void StartAuction(IOrderedEnumerable<Item> allItems, DynamoDBContext context)
		{
			var item = allItems.FirstOrDefault(i => i.Status == "Ready");
			if (item != null)
			{
				item.Timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
				item.Status = "Active";
				context.Save(item);
			}
		}

		public static string CreateBidQueue()
		{
			using (var sqs = new AmazonSQSClient())
			{
				var sqsRequest = new CreateQueueRequest { QueueName = auctionCode + "_Bid" };
				var createQueueResponse = sqs.CreateQueue(sqsRequest);
				return createQueueResponse.QueueUrl;
			}
		}

		private static bool AuctionFinished(IEnumerable<Item> items)
		{
			return items.All(i => i.Status == "Finished");
		}

		private static bool AuctionHasNotStarted(IEnumerable<Item> items)
		{
			return items.Any(i => i.Status == "Ready") && items.All(i => i.Status != "Active");
		}

		private static List<Bid> ReadAllBidsFromQueue()
		{
			using (var sqs = new AmazonSQSClient())
			{
				List<Bid> bids = new List<Bid>();
				var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = bidQueueUrl };
				var receiveMessageResponse = sqs.ReceiveMessage(receiveMessageRequest);
				if (receiveMessageResponse.Messages != null && receiveMessageResponse.Messages.Any())
				{
					foreach (var message in receiveMessageResponse.Messages)
					{
						try
						{

							bids.Add(JsonConvert.DeserializeObject<Bid>(message.Body));
						}
						catch
						{
							// ignored
						}

						var deleteRequest = new DeleteMessageRequest { QueueUrl = bidQueueUrl, ReceiptHandle = message.ReceiptHandle };
						sqs.DeleteMessage(deleteRequest);
					}
				}

				return bids;
			}
		}
	}
}