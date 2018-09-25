using Chat.Helper;
using Chat.Model.Common;
using Chat.Model.DTO.UserInfo;
using Chat.Service.IService;
using Chat.Service.Service;
using Chat.Ultilities.Loger;
using Microsoft.AspNet.SignalR;
using System.Text.RegularExpressions;
using System.Web;

namespace Chat
{
    public abstract class BaseHub : Hub
    {
        private ILoginService loginService = new LoginService();

        /// <summary>
        /// 当前登录的用户属性
        /// </summary>
        protected UserDTO CurrentUser
        {
            get
            {
                UserDTO rtn = null;
                HttpCookie cookie =new HttpCookie(Context.RequestCookies[CookieHelper.LoginCookieKey].Value);
                if (cookie != null && cookie.Name != null)
                {
                    string phone = Regex.Replace(cookie.Name, @"[^0-9]+", "");
                    var currentUser = loginService.GetUserInfoByMobile(phone);
                    if (currentUser.ResultType == OperationResultType.Success && currentUser.Content != null)
                    {
                        rtn=currentUser.Content;
                    }
                    else
                    {
                        Log.Error("BaseHub.CurrentUser获取用户为Null");
                    }
                }
                return rtn;
            }
        }
        
    }
}