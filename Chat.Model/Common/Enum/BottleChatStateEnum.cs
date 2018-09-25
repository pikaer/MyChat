using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.Common.Enum
{
    /// <summary>
    /// 漂流瓶互动状态
    /// </summary>
    public enum BottleChatStateEnum
    {
        [Description("初始状态")]
        Default = 0,
        [Description("主人删除")]
        UserDelete = 1,
        [Description("被捡到者删除")]
        OrigionDelete =2
    }
}
