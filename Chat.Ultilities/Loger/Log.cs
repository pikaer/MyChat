using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace Chat.Ultilities.Loger
{
    /// <summary>
    /// 日志操作管理类
    /// </summary>
    public class Log
    {
        static readonly ILog loginfo = LogManager.GetLogger("loginfo");
        static readonly ILog logerror = LogManager.GetLogger("logerror");
        static readonly ILog logmonitor = LogManager.GetLogger("logmonitor");
        static readonly ILog logperformance = LogManager.GetLogger("logperformance");

        #region 构造函数

        /// <summary>
        /// 对引入log的项目创建log4net.config配置文件
        /// </summary>
        static Log()
        {
            const string filename = "log4net.config";
            string basePath = HttpContext.Current != null ? AppDomain.CurrentDomain.SetupInformation.PrivateBinPath : AppDomain.CurrentDomain.BaseDirectory;
            string configFile = Path.Combine(basePath, filename);
            if (File.Exists(configFile))
            {
                XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
                return;
            }

            //默认设置
            RollingFileAppender appender1 = new RollingFileAppender
            {
                Name = "root",
                File = "Logs\\Log.txt",
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                DatePattern = "yyyyMMdd\".txt\"",
                MaxSizeRollBackups = 10
            };
            PatternLayout layout = new PatternLayout("[%d{yyyy-MM-dd HH:mm:ss.fff}] %c.%M %t %n%m%n");
            appender1.Layout = layout;
            BasicConfigurator.Configure(appender1);
            appender1.ActivateOptions();
            
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ErrorMsg">提示消息</param>
        /// <param name="ex">异常</param>
        public static void Error(string ErrorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                logerror.Error(ErrorMsg, ex);
            }
            else
            {
                logerror.Error(ErrorMsg);
            }
        }

        /// <summary>
        /// 性能记录日志
        /// </summary>
        /// <param name="controllername">控制器(类)名</param>
        /// <param name="actionname">方法名</param>
        /// <param name="msg">消息提示</param>
        public static void Performance(string controllername, string actionname, string msg)
        {
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendFormat("控制器(类)名：{0},方法名：{1},消息提示：{2}", controllername, actionname, msg);
            Performance(sbMsg.ToString());
        }

        /// <summary>
        /// 一般消息日志
        /// </summary>
        /// <param name="controllername">控制器(类)名</param>
        /// <param name="actionname">方法名</param>
        /// <param name="msg">消息提示</param>
        public static void Info(string controllername, string actionname, string msg)
        {
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendFormat("控制器(类)名：{0},方法名：{1},消息提示：{2}", controllername, actionname, msg);
            Debug(sbMsg.ToString());
        }


        /// <summary>
        /// 抛异常日志
        /// </summary>
        /// <param name="controllername">控制器(类)名</param>
        /// <param name="actionname">方法名</param>
        /// <param name="ex">异常内容</param>
        /// <param name="msg">消息提示</param>
        public static void Exception(string controllername, string actionname, Exception ex,string msg="")
        {
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendFormat("控制器(类)名：{0},方法名：{1},调用堆栈：{2},消息提示：{3}", controllername, actionname, ex.ToString(), msg);
            Exception(sbMsg.ToString());
        }
        #endregion

        #region 私有方法
        private static void Debug(string msg)
        {
            logmonitor.Debug(msg);
        }

        public static void Performance(string msg)
        {
            logperformance.Info(msg);
        }
        private static void Info(string Msg)
        {
            loginfo.Info(Msg);
        }
        private static void Exception(string msg)
        {
            logerror.Error(msg);
        }
        #endregion
    }
}
