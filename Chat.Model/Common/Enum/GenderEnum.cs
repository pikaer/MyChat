using System.ComponentModel;

namespace Chat.Model.Common.Enum
{
    public enum GenderEnum
    {
        [Description("不限")]
        Default = 0,
        [Description("男")]
        Man =1,
        [Description("女")]
        Woman = 2
    }
}
