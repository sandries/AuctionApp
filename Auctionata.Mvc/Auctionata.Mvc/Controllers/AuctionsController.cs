namespace Auctionata.Mvc.Controllers
{
	using Auctionata.Mvc.Services;
	using System.Web.Mvc;

	public class AuctionsController : Controller
	{
		// GET: Auctions
		public ActionResult Index()
		{
			var auctions = AuctionService.GetAllAuctions();
			this.Session["Auctions"] = auctions;
			this.ViewData["Auctions"] = auctions;
			return View();
		}
	}
}