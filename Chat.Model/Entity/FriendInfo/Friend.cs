using Chat.Model.Common.Enum;
using System;

namespace Chat.Model
{
    /// <summary>
    /// 好友实体类
    /// </summary>
    public class Friend
    {
        /// <summary>
        /// 主键,唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户Id,即这个朋友是谁的
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 根Id,即这个朋友在User中对应的Id
        /// </summary>
        public long OriginId { get; set; }

        /// <summary>
        /// 添加方式
        /// </summary>
        public AddTypeEnum AddType { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注名
        /// </summary>
        public string DescName { get; set; }

        /// <summary>
        /// 聊天列表删除记录时间
        /// </summary>

        public DateTime? DeleteChatContentTime { get; set; }

        /// <summary>
        /// 最新阅读聊天内容时间
        /// </summary>
        public DateTime? ReadTime { get; set; }
    }
}
