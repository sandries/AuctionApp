namespace Auctionata.Api.Models
{
	public class Bid
	{
		public string User { get; set; }

		public decimal Amount { get; set; }

		public int ItemId { get; set; }

		public string AuctionCode { get; set; }
	}
}