using Chat.Model.Common;
using Chat.Ultilities.Extensions;

namespace Chat.Model.DTO.Chat
{
    /// <summary>
    /// 聊天历史记录子类
    /// </summary>
    public class ChatHistoryDTO : PersonalChatHistory
    {
        public bool OnRight { get; set; }

        public long OriginId { get; set; }

        public string HeadshotPath { get; set; }

        /// <summary>
        /// 创建时间转义
        /// </summary>
        public string CreateTimeDesc
        {
            get
            {
                return DateTimeExtensions.GetDateDesc(CreateTime);
            }
        }

        /// <summary>
        /// 创建时间 转义
        /// </summary>
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.ToString();
            }
        }

        /// <summary>
        /// 头像地址(50像素）
        /// </summary>
        public string DefaultPath_50
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_50") : ConfigHelper.GetPath("DefaultPath_50", HeadshotPath);
            }
        }
    }
}
