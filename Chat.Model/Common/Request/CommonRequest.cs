using System;

namespace Chat.Model
{
    /// <summary>
    /// 公共请求类
    /// </summary>
    public class CommonRequest
    {
        /// <summary>
        /// 通用Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// User.Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 副本（Id)
        /// </summary>
        public long? BId { get; set; }
        /// <summary>
        /// Friend.Id
        /// </summary>
        public long? FriendId { get; set; }

        public long? OriginId { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        /// <summary>
        /// ChatHistory.Id
        /// </summary>
        public Guid ChatHistoryId { get; set; }

        /// <summary>
        /// 通用内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        
    }
}
