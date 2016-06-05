using System.Web.Http;

namespace Auctionata.Api
{
	using System.Net.Http.Headers;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{code}",
				defaults: new { code = RouteParameter.Optional }
			);
		}
	}
}
