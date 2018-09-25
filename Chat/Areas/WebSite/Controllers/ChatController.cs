using Chat.Helper;
using Chat.Model;
using Chat.Model.Common;
using Chat.Model.Common.Enum;
using Chat.Model.DTO.Chat;
using Chat.Service.IService;
using Chat.Service.Service;
using System.Web.Mvc;

namespace Chat.Areas.WebSite.Controllers
{
    /// <summary>
    /// 聊天模块控制器
    /// </summary>
    public class ChatController : BaseController
    {
        private IChatContentService chatContentService = new ChatContentService();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 向数据库插入聊天记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult AddPersonalChatHistory(string data)
        {
            var request = data.JsonToObject<ChatHistoryDTO>();
            request.Type = ChatContentEnum.Text;
            var response = chatContentService.AddPersonalChatHistory(request,CurrentUserInfo.Id);
            return Json(response.JsonMess());
        }

        /// <summary>
        /// 获取聊天好友记录列表
        /// </summary>
        public JsonResult GetChatFriendList()
        {
            var rtn = chatContentService.GetChatFriendList(CurrentUserInfo.Id);
            return Json(rtn, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取对方发来的消息内容
        /// </summary>
        public JsonResult GetChatContent(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var response = chatContentService.GetChatContent(request);
            return Json(response.JsonMess());
        }

        /// <summary>
        /// 获取聊天记录
        /// </summary>
        public JsonResult GetChatContentHistory(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var response = chatContentService.GetChatContentHistory(request);
            return Json(response.JsonMess());
        }
       
        /// <summary>
        /// 刷新未读消息条数
        /// </summary>
        public JsonResult RefrashUnReadMsg()
        {
            var response = chatContentService.RefrashUnReadMsg(CurrentUserInfo.Id);
            return Json(response.JsonMess());
        }

        /// <summary>
        /// 将未读标记为已读
        /// </summary>
        public JsonResult UpdateReadTime(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var response = chatContentService.UpdateReadTime(request);
            return Json(response.JsonMess());
        }
    }
}