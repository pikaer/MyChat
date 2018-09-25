using Chat.Model.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.Entity.Discovery
{
    public class Bottle
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 瓶子主人
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 被捡者Id
        /// </summary>
        public long? OriginId { get; set; }

        /// <summary>
        /// 瓶子内容
        /// </summary>
        public string BottleContent { get; set; }

        /// <summary>
        /// 漂流瓶状态
        /// </summary>
        public BottleStateEnum BottleState { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime{get;set;}

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
