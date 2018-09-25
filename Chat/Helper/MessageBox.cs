using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Chat.Helper
{
    public class MessageBox
    {
        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(string msg, string url, string target = "")
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script>");
            Builder.AppendFormat("alert('{0}');", msg);
            if (target != "")
            {
                target = "." + target;
            }
            Builder.AppendFormat("window{0}.location.href='{1}'", target, url);
            Builder.Append("</script>");
            {
                //不能用page.ClientScript.RegisterStartupScript，因为如果这样ShowAndRedirect调用的地方的后续代码还会接着走下去。
                //因此必须用如下方式进行终止
                HttpContext.Current.Response.Write(Builder.ToString());
                HttpContext.Current.Response.End();
            }
        }

        #region MVC
        public static ContentResult GetShowAndRedirect(string msg, bool historyBack = true, string url = "", string target = "")
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script>");
            Builder.AppendFormat("alert('{0}');", msg);
            if (historyBack)
            {
                Builder.Append("history.back();");
            }
            else
            {
                if (target != "")
                {
                    target = "." + target;
                }

                Builder.AppendFormat("window{0}.location.href='{1}'", target, url);

            }
            Builder.Append("</script>");
            return new ContentResult() { Content = Builder.ToString() };
        }

        public static ContentResult GetConfirm(string msg, string yesUrl, bool noHistoryBack = true, string noUrl = "")
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script>");
            Builder.Append("if(confirm('" + msg + "')){");
            Builder.AppendFormat("window.location.href='{0}'}}", yesUrl);
            Builder.Append("else{");
            if (noHistoryBack)
            {
                Builder.Append("history.back();}");
            }
            else
            {
                Builder.AppendFormat("window.location.href='{0}'}}", noUrl);
            }
            Builder.Append("</script>");
            return new ContentResult() { Content = Builder.ToString() };
        }

        public static ContentResult Redirect(string msg, string yesUrl, bool noHistoryBack = true, string noUrl = "")
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script>");
            Builder.Append("if(confirm('" + msg + "')){");
            Builder.AppendFormat("window.location.href='{0}'}}", yesUrl);
            Builder.Append("else{");
            if (noHistoryBack)
            {
                Builder.Append("history.back();}");
            }
            else
            {
                Builder.AppendFormat("window.location.href='{0}'}}", noUrl);
            }
            Builder.Append("</script>");
            return new ContentResult() { Content = Builder.ToString() };
        }
        #endregion
    }
}