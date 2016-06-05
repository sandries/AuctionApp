namespace Auctionata.Mvc.Controllers
{
	using System;
	using Auctionata.Mvc.Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using Auctionata.Mvc.Services;

	public class AuctionController : Controller
	{
		public ActionResult Index(string code)
		{
			var auctions = this.Session["Auctions"] as List<Auction>;
			if (auctions == null)
			{
				auctions = AuctionService.GetAllAuctions();
				this.Session["Auctions"] = auctions;
			}
			var auction = auctions.FirstOrDefault(a => a.Code == code);

			var items = this.Session[code] as List<Item>;
			if (items == null)
			{
				items = AuctionService.GetAuctionItems(code);
				this.Session[code] = items;
			}

			if (this.Session["RegistredInAuctions"] == null) this.Session["RegistredInAuctions"] = new List<string>();
			bool registered = ((List<string>)this.Session["RegistredInAuctions"]).Contains(auction.Code);
			var model = new AuctionDetailViewModel
			{
				Auction = auction,
				Items = items,
				Registred = registered,
				ActiveItem = registered ? AuctionService.GetCurrentlyAuctionedItem(auction.Code) : null
			};
			return this.View(model);

		}

		[HttpPost]
		public ActionResult Register(FormCollection collection)
		{
			var auctionCode = Convert.ToString(collection["Auction.Code"]);
			var email = Convert.ToString(collection["email"]);
			this.Session["email"] = email;
			AuctionService.RegisterUserForAuction(email, auctionCode);

			if (this.Session["RegistredInAuctions"] == null) this.Session["RegistredInAuctions"] = new List<string>();
			((List<string>)this.Session["RegistredInAuctions"]).Add(auctionCode);

			return RedirectToAction("Index", new { code = auctionCode });
		}

		[HttpPost]
		public ActionResult Bid(FormCollection collection)
		{
			var auctionCode = Convert.ToString(collection["Auction.Code"]);
			var itemId = Convert.ToInt32(collection["ActiveItem.Id"]);
			var amount = Convert.ToDecimal(collection["ActiveItem.AskingPrice"]);
			var email = Convert.ToString(this.Session["email"]);

			AuctionService.MakeBid(auctionCode, itemId, amount, email);
			return RedirectToAction("Index", new { code = auctionCode });
		}
	}
}
