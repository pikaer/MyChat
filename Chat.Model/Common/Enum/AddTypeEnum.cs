using System.ComponentModel;

namespace Chat.Model.Common.Enum
{
    /// <summary>
    /// 好友添加方式
    /// </summary>
    public enum AddTypeEnum
    {
        /// <summary>
        ///搜索
        /// </summary>
        [Description("搜索")]
        Search = 0,
        /// <summary>
        ///语音
        /// </summary>
        [Description("附近")]
        Near = 1,
        /// <summary>
        ///表情
        /// </summary>
        [Description("漂流瓶")]
        Bottle = 2
    }
}
