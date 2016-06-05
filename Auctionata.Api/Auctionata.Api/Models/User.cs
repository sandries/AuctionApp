namespace Auctionata.Api.Models
{
	using Amazon.DynamoDBv2.DataModel;

	[DynamoDBTable("Users")]
	public class User
	{
		[DynamoDBProperty(AttributeName = "Email")]
		public string Email { get; set; }

		[DynamoDBProperty(AttributeName = "AuctionCode")]
		public string AuctionCode { get; set; }
	}
}