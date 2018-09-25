using Chat.Model.Common.Enum;
using System;

namespace Chat.Model
{
    /// <summary>
    /// 聊天记录
    /// </summary>
    public class PersonalChatHistory
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 朋友Id,是和那个朋友的聊天记录(Friend_Id)
        /// </summary>
        public long FriendId { get; set; }
        /// <summary>
        /// 本条聊天内容
        /// </summary>
        public string ChatContent { get; set; }

        /// <summary>
        /// 聊天内容类别
        /// </summary>
        public ChatContentEnum Type { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }

        
    }
}
