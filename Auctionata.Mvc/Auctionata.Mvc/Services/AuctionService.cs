namespace Auctionata.Mvc.Services
{
	using System.IO;
	using System.Net;
	using Auctionata.Mvc.Models;
	using Newtonsoft.Json;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Newtonsoft.Json.Linq;

	public static class AuctionService
	{
		public static List<Auction> GetAllAuctions()
		{
			return GetList<Auction>(@"http://localhost:12179/api/auctions");
		}

		public static List<Item> GetAuctionItems(string code)
		{
			return GetList<Item>($"http://localhost:12179/api/auction/{code}?onlyActive=false");
		}

		public static void RegisterUserForAuction(string email, string code)
		{
			dynamic json = new JObject();
			json.Email = email;
			json.Code = code;
			PostJson(json, "http://localhost:12179/api/users");
		}

		public static Item GetCurrentlyAuctionedItem(string code)
		{
			return GetList<Item>($"http://localhost:12179/api/auction/{code}?onlyActive=true").FirstOrDefault();
		}

		public static void MakeBid(string auctionCode, int itemId, decimal amount, string email)
		{
			dynamic json = new JObject();
			json.User = email;
			json.Amount = amount;
			json.ItemId = itemId;
			json.AuctionCode = auctionCode;

			PostJson(json, $"http://localhost:12179/api/auction");
		}

		private static List<T> GetList<T>(string url)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = WebRequestMethods.Http.Get;
			request.Accept = "application/json";
			var response = (HttpWebResponse)request.GetResponse();

			var ret = new List<T>();
			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				string json = sr.ReadToEnd();
				if (string.IsNullOrWhiteSpace(json) || json == "[null]")
				{
					return ret;
				}
				ret.AddRange((JsonConvert.DeserializeObject<List<T>>(json)));
			}
			return ret;
		}

		private static string PostJson(dynamic json, string url)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);

			var postData = JsonConvert.SerializeObject(json);
			var data = Encoding.ASCII.GetBytes(postData);

			request.Method = "POST";
			request.ContentType = "application/json";
			request.ContentLength = postData.Length;

			using (var stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			var response = (HttpWebResponse)request.GetResponse();
			return new StreamReader(response.GetResponseStream()).ReadToEnd();
		}
	}
}
