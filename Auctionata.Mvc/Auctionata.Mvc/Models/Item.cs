namespace Auctionata.Mvc.Models
{
	using System.Runtime.Remoting.Activation;

	public class Item
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Image { get; set; }

		public decimal HighestBid { get; set; }

		public decimal AskingPrice { get; set; }

		public decimal StartingPrice { get; set; }

		public string User { get; set; }

		public string Description { get; set; }
	}
}
