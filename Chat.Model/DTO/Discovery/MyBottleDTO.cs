using Chat.Model.Common;
using Chat.Model.Common.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.DTO.Discovery
{
    /// <summary>
    /// 我的瓶子列表
    /// </summary>
    public class MyBottleDTO
    {
        /// <summary>
        /// 漂流瓶Id
        /// </summary>
        public long BottleId { get; set; }

        /// <summary>
        /// 漂流瓶对话Id
        /// </summary>
        public long BottleChatId { get; set; }

        /// <summary>
        /// 瓶子主人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 漂流瓶主人昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 漂流瓶状态
        /// </summary>
        public BottleStateEnum BottleState { get; set; }

        /// <summary>
        /// 最新瓶子消息
        /// </summary>
        public string RecentContent { get; set; }

        /// <summary>
        /// 最新时间
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 漂流瓶主人所在城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 漂流瓶主人头像
        /// </summary>
        public string HeadshotPath { get; set; }

        /// <summary>
        /// 最近一天聊天记录创建时间转义
        /// </summary>
        public string LastTimeDesc
        {
            get
            {
                return LastTime.ToShortTimeString();
            }
        }

        public string HeadshotPathDesc
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_50") : ConfigHelper.GetPath("DefaultPath_50", HeadshotPath);
            }
        }
    }
}
