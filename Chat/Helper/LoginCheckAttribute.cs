using Chat.Ultilities.Loger;
using System.Diagnostics;
using System.Threading;
using System.Web.Mvc;

namespace Chat.Helper
{
    public class UserCheckAttribute: ActionFilterAttribute
    {
        Stopwatch wat = new Stopwatch();
        Stopwatch swAsync = new Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            swAsync.Reset();
            swAsync.Start();
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoLoginCheck), true);
            if (attrs.Length != 1)
            {
                if (filterContext.HttpContext.Session != null)
                {
                    if (filterContext.HttpContext.Session[CookieHelper.SessionUserKey] == null)
                    {
                        MessageBox.ShowAndRedirect("非法访问或者访问已过期，请重新登录系统!", "/Login/Index", "top");
                        return;
                    }
                }
            }
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            swAsync.Stop();
            if (swAsync.ElapsedMilliseconds > 0)
            {
                string msg = string.Format("页面{0},线程id={1},Action执行时间{2}毫秒", filterContext.HttpContext.Request.RawUrl, Thread.CurrentThread.ManagedThreadId, swAsync.ElapsedMilliseconds);
                Log.Performance(msg);
            }
        }
    }
}