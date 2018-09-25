using Chat.Helper;
using Chat.Model;
using Chat.Model.Common;
using Chat.Model.Entity.Discovery;
using Chat.Service.IService;
using Chat.Service.Service;
using System.Web.Mvc;

namespace Chat.Areas.WebSite.Controllers
{
    /// <summary>
    /// 发现模块控制器
    /// </summary>
    public class DiscoveryController : BaseController
    {
        private IBottleService discoveryService = new BottleService();

        public ActionResult Index()
        {
            return View();
        }

        #region 漂流瓶
        /// <summary>
        /// 扔一个瓶子
        /// </summary>
        /// <returns></returns>
        public JsonResult ThrowOneBottle(string data)
        {
            var request = data.JsonToObject<Bottle>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = discoveryService.ThrowOneBottle(request);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 捡一个瓶子
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNewBottle()
        {
            var rtn = discoveryService.NewBottle(CurrentUserInfo.Id);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 我捡过的瓶子列表
        /// </summary>
        public JsonResult GetAllBottleList()
        {
            var rtn = discoveryService.AllBottleList(CurrentUserInfo.Id);
            return Json(rtn.JsonMess()); 
        }

        /// <summary>
        /// 某一个瓶子回复列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult ReplyBottleList(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = discoveryService.ReplyBottleList(request);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 回复某一个瓶子
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult ReplyBottle(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = discoveryService.ReplyBottle(request);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 我的回复过的和我扔出去且被回复的瓶子列表
        /// </summary>
        public JsonResult AllReplyBottleList()
        {
            var rtn = discoveryService.AllReplyBottleList(CurrentUserInfo.Id);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 删除或者举报瓶子(一级列表）
        /// </summary>
        public JsonResult UpdateBottle(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = discoveryService.UpdateBottle(request);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 删除我的瓶子（二级列表）
        /// </summary>
        public JsonResult DeleteBottleReply(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = discoveryService.DeleteBottleReply(request);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 删除漂流瓶对话（三级列表）
        /// </summary>
        public JsonResult DeleteBottleChat(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUserInfo.Id;
            var rtn = discoveryService.DeleteBottleChat(request);
            return Json(rtn.JsonMess());
        }
        #endregion
    }
}