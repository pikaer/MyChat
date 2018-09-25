using System.ComponentModel;

namespace Chat.Model.Common.Enum
{
    public enum ChatContentEnum
    {
        /// <summary>
        ///文本
        /// </summary>
        [Description("文本")]
        Text=0,
        /// <summary>
        ///语音
        /// </summary>
        [Description("语音")]
        Voice=1,
        /// <summary>
        ///表情
        /// </summary>
        [Description("表情")]
        Expression=2,
        /// <summary>
        ///视频
        /// </summary>
        [Description("视频")]
        Video=3,
        /// <summary>
        ///文件
        /// </summary>
        [Description("文件")]
        File=4,
    }
}
