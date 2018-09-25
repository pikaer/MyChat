using System.ComponentModel;

namespace Chat.Model.Common.Enum
{
    /// <summary>
    /// 年龄分段
    /// </summary>
    public enum AgeEnum
    {
        [Description("不限")]
        Default = 0,
        [Description("18岁以下")]
        Down_18 = 1,
        [Description("18-22岁")]
        _18_22 = 2,
        [Description("23-26岁")]
        _23_26 = 3,
        [Description("27-35岁")]
        _27_35 = 4,
        [Description("35岁以上")]
        _35Up = 5
    }
}
