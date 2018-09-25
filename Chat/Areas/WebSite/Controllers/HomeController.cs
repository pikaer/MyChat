using Chat.Service.IService;
using Chat.Service.Service;
using System.Web.Mvc;

namespace Chat.Areas.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        private IUserService userService = new UserService();
        // GET: WebSite/Home
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 用户默认信息
        /// </summary>
        public JsonResult GetUserDefaultInfo()
        {
            var rtn = userService.GetBasicInfo(CurrentUserInfo.Id);
            return Json(rtn.JsonMess());
        }
    }
}