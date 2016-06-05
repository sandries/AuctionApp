namespace Auctionata.Api.Controllers
{
	using Amazon.DynamoDBv2;
	using Amazon.DynamoDBv2.DataModel;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;
	using Auctionata.Api.Models;

	public class AuctionsController : ApiController
	{
		public IEnumerable<Auction> Get()
		{
			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				return context.Scan<Auction>().OrderBy(o => o.StartingDate).ToList();
			}
		}

		public Auction Get(string code)
		{
			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				return context.Scan<Auction>().FirstOrDefault(a => a.Code == code);
			}
		}
	}
}
