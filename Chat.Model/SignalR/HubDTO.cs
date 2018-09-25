using System;
namespace Chat.Model
{
    /// <summary>
    /// 用户=>连接 映射类
    /// </summary>
    public class HubDTO
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 连接Id(动态生成）
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }
    }
}
