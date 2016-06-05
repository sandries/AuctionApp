namespace Auctionata.Api.Models
{
	using Amazon.DynamoDBv2.DataModel;

	[DynamoDBTable("Auctions")]
	public class Auction
	{
		[DynamoDBProperty(AttributeName = "Code")]
		public string Code { get; set; }

		[DynamoDBProperty(AttributeName = "Name")]
		public string Name { get; set; }

		[DynamoDBProperty(AttributeName = "Description")]
		public string Description { get; set; }

		[DynamoDBProperty(AttributeName = "StartingDate")]
		public string StartingDate { get; set; }

		[DynamoDBProperty(AttributeName = "Picture")]
		public string Picture { get; set; }
	}
}
