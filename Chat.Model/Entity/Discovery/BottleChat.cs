using Chat.Model.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.Entity.Discovery
{
    /// <summary>
    /// 漂流瓶互动
    /// </summary>
    public class BottleChat
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 漂流瓶Id
        /// </summary>
        public long BottleId { get; set; }

        /// <summary>
        /// 瓶子主人，若为0，则是捡到者回复
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 捡到者Id,若为0，则是主人回复
        /// </summary>
        public long? OriginId { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string BottleChatContent { get; set; }

        /// <summary>
        /// 该条对话状态
        /// </summary>
        public BottleChatStateEnum ChatState { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
