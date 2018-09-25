using Chat.Helper;
using Chat.Model.Common;
using Chat.Model.Common.Helper;
using Chat.Model.DTO.UserInfo;
using Chat.Service.IService;
using Chat.Service.Service;
using Chat.Ultilities.Extensions;
using System;
using System.Web;
using System.Web.Mvc;

namespace Chat.Areas.WebSite.Controllers
{
    /// <summary>
    /// 用户登录管理
    /// </summary>
    public class LoginController : BaseController
    {
        private ILoginService loginService = new LoginService();

        [NoLoginCheck]
        public ActionResult Index()
        {
            if (Session[CookieHelper.SessionUserKey] == null && Session[CookieHelper.SessionRoleFuncKey] == null)
            {
                //先判断cookie是否有存储已登录信息，如果有赋值给session并直接跳转到主页
                HttpCookie cookie = Request.Cookies[CookieHelper.LoginCookieKey];
                if (cookie != null && cookie[CookieHelper.LoginCookieNameKey] != null)
                {
                    var currentUser = loginService.GetUserInfoByMobile(cookie[CookieHelper.LoginCookieNameKey]);
                    if(currentUser.ResultType== OperationResultType.Success&& currentUser.Content!=null)
                    {
                        Session[CookieHelper.SessionUserKey] = currentUser.Content;
                        Session.Timeout = 60;
                        return RedirectToAction("Index", "Chat");
                    }
                }
            }
            return View();
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        [HttpPost]
        [NoLoginCheck]
        public JsonResult Login(string data)
        {
            var request = data.JsonToObject<UserDTO>();
            string pwd = string.IsNullOrEmpty(request.PassWord)?"": Md5.GetMd5Str32(request.PassWord);
            var currentUser = loginService.GetUserInfoByMobile(request.Mobile);
            if (currentUser.ResultType == OperationResultType.Success && currentUser.Content != null&& currentUser.Content.PassWord.ToLower().Equals(pwd))
            {
                Session[CookieHelper.SessionUserKey] = currentUser.Content;
                Session.Timeout = 60;
                if (request.RememberMe)
                {
                    HttpCookie cookie = Request.Cookies[CookieHelper.LoginCookieKey];
                    if (cookie == null)
                    {
                        cookie = new HttpCookie(CookieHelper.LoginCookieKey);
                    }
                    cookie[CookieHelper.LoginCookieNameKey] = request.Mobile;
                    cookie.Expires = DateTime.Now.AddDays(3);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie cookie_temp = Request.Cookies[CookieHelper.LoginCookieKey];
                    if (cookie_temp != null)
                    {
                        cookie_temp.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cookie_temp);
                    }
                }
                return Json(new JsonMess(true, "").ToJson());
            }
            else
            {
                return Json(new JsonMess(false, "用户名或登录密码有误!").ToJson());
            }
        }

        /// <summary>
        /// 用户注册账户
        /// </summary>
        public JsonResult Register(string data)
        {
            var request = data.JsonToObject<UserDTO>();
            var response = new OperationResult<bool>();
            var hasMobile= loginService.GetUserInfoByMobile(request.Mobile);
            if(hasMobile.ResultType== OperationResultType.Success && hasMobile.Content!=null)
            {
                response.Message = "该手机号已注册！";
                response.ResultType = OperationResultType.ParamError;
                response.Content = false;
            }
            else
            {
                var rtn = loginService.Register(request);
                if (rtn.ResultType == OperationResultType.Success && rtn.Content > 0)
                {
                    var currentUser = loginService.GetUserInfoById(rtn.Content);
                    Session[CookieHelper.SessionUserKey] = currentUser.Content;
                    Session.Timeout = 60;
                }
                response.Message = "注册成功！";
                response.ResultType = OperationResultType.Success;
                response.Content = true;
            }
            
            return Json(response.JsonMess());
        }
    }
}