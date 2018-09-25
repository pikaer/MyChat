using System.ComponentModel;

namespace Chat.Model.Common.Enum
{
    public enum ProvinceTypeEnum
    {
        [Description("常规")]
        Default = 0,
        [Description("直辖市")]
        ZhiXia = 1,
        [Description("自治区")]
        ZiZhi = 2,
    }
}
