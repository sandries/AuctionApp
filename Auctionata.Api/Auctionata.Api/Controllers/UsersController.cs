namespace Auctionata.Api.Controllers
{
	using System.Linq;
	using Amazon.DynamoDBv2;
	using Amazon.DynamoDBv2.DataModel;
	using System.Collections.Generic;
	using System.Web.Http;
	using Auctionata.Api.Models;

	public class UsersController : ApiController
	{
		public IEnumerable<User> Get()
		{
			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				return context.Scan<User>().ToList();
			}
		}

		public User Get(string code)
		{
			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				return context.Scan<User>().FirstOrDefault(u => u.AuctionCode == code);
			}
		}

		public void Post([FromBody]dynamic data)
		{
			using (var client = new AmazonDynamoDBClient())
			{
				var context = new DynamoDBContext(client);
				context.Save(new User { Email = data.Email, AuctionCode = data.Code });
			}
		}
	}
}
