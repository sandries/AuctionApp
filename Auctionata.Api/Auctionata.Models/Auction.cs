namespace Auctionata.Models
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

		[DynamoDBProperty(AttributeName = "StartDate")]
		public string StartDate { get; set; }
	}

	[DynamoDBTable("Items")]
	public class Item
	{
		[DynamoDBProperty(AttributeName = "Id")]
		public int Id { get; set; }

		[DynamoDBProperty(AttributeName = "AuctionCode")]
		public string AuctionCode { get; set; }

		[DynamoDBProperty(AttributeName = "Name")]
		public string Name { get; set; }

		[DynamoDBProperty(AttributeName = "Description")]
		public string Description { get; set; }

		[DynamoDBProperty(AttributeName = "HighestBid")]
		public decimal HighestBid { get; set; }

		[DynamoDBProperty(AttributeName = "StartingPrice")]
		public decimal StartingPrice { get; set; }

		[DynamoDBProperty(AttributeName = "Image")]
		public string Image { get; set; }

		[DynamoDBProperty(AttributeName = "Status")]
		public string Status { get; set; }

		[DynamoDBProperty(AttributeName = "User")]
		public string User { get; set; }

		[DynamoDBProperty(AttributeName = "Timestamp")]
		public string Timestamp { get; set; }
	}
}
