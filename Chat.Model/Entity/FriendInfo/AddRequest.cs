using Chat.Model.Common.Enum;
using System;

namespace Chat.Model.Entity.FriendInfo
{
    public class AddRequest
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 添加好友发起者Id
        /// </summary>
        public long ReqUserId { get; set; }

        /// <summary>
        /// 被添加着Id
        /// </summary>
        public long ReqOriginId { get; set; }

        /// <summary>
        /// 添加方式
        /// </summary>
        public AddTypeEnum AddType { get; set; }

        /// <summary>
        /// 好友添加状态
        /// </summary>
        public AddStatEnum AddStat { get; set; }

        /// <summary>
        /// 添加好友验证信息
        /// </summary>
        public string ReqContent { get; set; }

        /// <summary>
        /// 备注名
        /// </summary>
        public string DescName { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最新修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Id=ReqUserId的用户逻辑删除了请求数据
        /// </summary>
        public bool IsUserDelete { get; set; }

        /// <summary>
        /// Id=ReqOriginId的用户逻辑删除了请求数据
        /// </summary>
        public bool IsOriginDelete { get; set; }
    }
}
