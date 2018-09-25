using Chat.Model;
using Chat.Model.Common.Request;
using Chat.Service.IService;
using Chat.Service.Service;
using System.Web;
using Chat.Helper;
using System.Web.Mvc;
using Chat.Model.Common;
using Chat.Model.DTO.UserInfo;

namespace Chat.Areas.WebSite.Controllers
{
    public class UserInfoController : BaseController
    {
        private IUserService userService = new UserService();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetUserList()
        {
            var dto = userService.GetUserList();
            return Json(new BootstrapTableResponse<User>() { rows = dto.Content, total = dto.Content.Count }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpLoadHeadPhoto(FormCollection form)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                //文件临时存放路径
                string saveDirect = Server.MapPath(("upfiles"));//Server.MapPath 获得虚拟服务器相对路径
                if (!System.IO.Directory.Exists(saveDirect))
                {
                    System.IO.Directory.CreateDirectory(saveDirect);
                }
                string savePath = saveDirect + "\\" + hfc[0].FileName;
                hfc[0].SaveAs(savePath);
                var request = form[1].JsonToObject<UpLoadPhotoRequest>();
                request.Path = savePath;
                var rtn = userService.UpLoadHeadPhoto(request, CurrentUserInfo.Id,saveDirect);
                return Json(rtn.JsonMess());
            }
            return null;
        }

        /// <summary>
        /// 用户详情页
        /// </summary>
        public JsonResult GetBasicInfo()
        {
            var rtn = userService.GetBasicInfo(CurrentUserInfo.Id);
            return Json(rtn.JsonMess());
        }

        /// <summary>
        /// 编辑用户详情
        /// </summary>
        public JsonResult BasicInfoEdit(string data)
        {
            var request= data.JsonToObject<BasicInfoDTO>();
            request.Id = CurrentUserInfo.Id;
            var rtn = userService.BasicInfoEdit(request);
            return Json(rtn.JsonMess());
        }
    }
}