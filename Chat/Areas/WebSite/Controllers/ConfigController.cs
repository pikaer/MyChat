using System.Web.Mvc;

namespace Chat.Areas.WebSite.Controllers
{
    public class ConfigController : Controller
    {
        // GET: WebSite/Config
        public ActionResult Index()
        {
            return View();
        }
    }
}