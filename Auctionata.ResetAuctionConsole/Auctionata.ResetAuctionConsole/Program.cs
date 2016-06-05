
namespace Auctionata.ResetAuctionConsole
{
	using Amazon.DynamoDBv2;
	using Amazon.DynamoDBv2.DataModel;
	using Amazon.DynamoDBv2.DocumentModel;

	public class Program
	{
		public static void Main(string[] args)
		{
			var auctionCode = args[0];
			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				var allItems = context.Scan<Item>(new ScanCondition("AuctionCode", ScanOperator.BeginsWith, auctionCode));

				foreach (var item in allItems)
				{
					item.Status = "Ready";
					item.HighestBid = 0;
					item.Timestamp = "null";
					item.User = "None";
					context.Save(item);
				}
			}
		}
	}
}