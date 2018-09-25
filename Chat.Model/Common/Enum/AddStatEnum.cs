using System.ComponentModel;

namespace Chat.Model.Common.Enum
{
    /// <summary>
    /// 好友添加状态
    /// </summary>
    public enum AddStatEnum
    {
        [Description("等待验证")]
        Wait = 0,
        [Description("已添加")]
        Pass = 1,
        [Description("已拒绝")]
        Refuse = 2,
        [Description("已忽略")]
        Ignore = 3,
        [Description("提问")]
        Question =4
        
    }
}
