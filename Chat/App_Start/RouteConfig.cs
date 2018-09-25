using System.Web.Mvc;
using System.Web.Routing;

namespace Chat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Login.Controllers" }
               ).DataTokens.Add("Area", "WebSite");
        }
    }
}
