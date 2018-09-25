using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.Common
{
    public static class ConfigHelper
    {
        public static string GetConfig(string Key)
        {
            string value = ConfigurationManager.AppSettings[Key];
            return string.IsNullOrEmpty(value) ? "" : value;
        }

        public static string GetPath(string key,string path)
        {
            string value = ConfigurationManager.AppSettings[key];
            string rtn = value + path+ ".png";
            return rtn;
        }
    }
}
