using Chat.Model.Entity.FriendInfo;
using Chat.Service.IService;
using Chat.Service.Service;
using System.Web.Mvc;
using Chat.Helper;
using Chat.Model.Common;
using Chat.Model.Common.Request;

namespace Chat.Areas.WebSite.Controllers
{
    public class AddController : BaseController
    {
        private IAddService addService = new AddService();
        // GET: WebSite/Add
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUserList(AddFriendRequest data)
        {
            var rtn = addService.GetUserList(data, CurrentUserInfo.Id);
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddFriend(string data)
        {
            var request = data.JsonToObject<AddRequest>();
            request.ReqUserId = CurrentUserInfo.Id;
            var response = addService.AddFriend(request);
            return Json(response.JsonMess());
        }
    }
}