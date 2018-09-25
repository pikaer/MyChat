using Chat.Helper;
using Chat.Model.DTO.UserInfo;
using System.Web.Mvc;

namespace Chat.Areas.WebSite.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前登录的用户属性
        /// </summary>
        public UserDTO CurrentUserInfo
        {
            get
            {
                return Session[CookieHelper.SessionUserKey] as UserDTO;
            }
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            // 当自定义显示错误 mode = On，显示友好错误页面
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                View("Error").ExecuteResult(this.ControllerContext);
            }
        }
    }
}