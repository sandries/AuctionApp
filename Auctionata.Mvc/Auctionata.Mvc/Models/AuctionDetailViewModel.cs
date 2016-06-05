namespace Auctionata.Mvc.Models
{
	using System.Collections.Generic;

	public class AuctionDetailViewModel
	{
		public Auction Auction { get; set; }

		public List<Item> Items { get; set; }

		public bool Registred { get; set; }

		public Item ActiveItem { get; set; }
	}
}
