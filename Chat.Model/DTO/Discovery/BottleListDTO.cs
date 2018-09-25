using Chat.Model.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.DTO.Discovery
{
    public class BottleListDTO
    {
        /// <summary>
        /// 漂流瓶Id
        /// </summary>
        public long BottleId { get; set; }

        /// <summary>
        /// 瓶子主人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 瓶子内容
        /// </summary>
        public string BottleContent { get; set; }

        /// <summary>
        /// 漂流瓶状态
        /// </summary>
        public BottleStateEnum BottleState { get; set; }

        /// <summary>
        /// 漂流瓶主人昵称
        /// </summary>
        public string NickName { get; set; }

   
        /// <summary>
        /// 漂流瓶主人性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 漂流瓶主人所在城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 瓶子捡到时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
