using Chat.Helper;
using Chat.Model;
using Chat.Model.Common;
using Chat.Model.DTO.Friend;
using Chat.Service;
using Chat.Service.IService;
using Chat.Service.Service;
using System.Web.Mvc;

namespace Chat.Areas.WebSite.Controllers
{
    /// <summary>
    /// 好友模块控制器
    /// </summary>
    public class FriendController : BaseController
    {
        private IFriendService friendService = new FriendService();
        private IUserService userService = new UserService();
        private IAddService addService = new AddService();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        public JsonResult GetFriendList()
        {
            var rtn = friendService.GetFriendList(CurrentUserInfo.Id);
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取新好友列表
        /// </summary>
        public JsonResult GetNewFriendList()
        {
            var rtn = friendService.GetNewFriendList(CurrentUserInfo.Id);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 添加好友验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult AddValidate(string data)
        {
            var request = data.JsonToObject<AddRequestDTO>();
            request.ReqUserId = CurrentUserInfo.Id;
            var rtn = friendService.AddValidate(request);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 好友详情
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult UserDetail(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = userService.UserDetail(request);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        public JsonResult DeleteFriend(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = userService.DeleteFriend(request);
            return Json(rtn.JsonMess());
        }

        public JsonResult DeleteAddRequest(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = addService.DeleteAddRequest(request);
            return Json(rtn.JsonMess());
        }
    }
}