using Chat.Model.Common;
using Chat.Model.Common.Enum;
using Chat.Ultilities.Extensions;
using System;
using System.Configuration;

namespace Chat.Model.DTO.Chat
{
    public class ChatFriendDTO
    {
        #region get;set;
        /// <summary>
        /// 最近一天聊天记录创建时间
        /// </summary>
        public DateTime? RecentChatTime { get; set; }

        /// <summary>
        /// 最近一次聊天内容
        /// </summary>
        public string RecentChatContent { get; set; }

        public ChatContentEnum Type { get; set; }

        public long? FriendId { get; set; }

        /// <summary>
        /// 好友备注名
        /// </summary>
        public string DescName { get; set; }

        /// <summary>
        /// 用户Id,即这个朋友是谁的
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 根Id,即这个朋友在User中对应的Id
        /// </summary>
        public long OriginId { get; set; }

        /// <summary>
        /// 未读消息条数
        /// </summary>
        public long? UnReadCount { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadshotPath { get; set; }
        #endregion

        #region get;

        /// <summary>
        /// 最近一次聊天内容
        /// </summary>
        public string RecentChatContentDesc 
        {
            get
            {
                string cotent="";
                if(Type==ChatContentEnum.Text)
                {
                    cotent= RecentChatContent;
                }
                else
                {
                    cotent = Type.ToDescription();
                }
                if (cotent!=null&&cotent.Length>5)
                {
                    cotent = cotent.Substring(0, 5) + "...";
                }
                return cotent;
            }
        }
        /// <summary>
        /// 最近一天聊天记录创建时间转义
        /// </summary>
        public string RecentChatTimeDesc
        {
            get
            {
                return RecentChatTime.HasValue?RecentChatTime.Value.ToShortTimeString():"";
            }
        }

        public string UnReadCountStr
        {
            get
            {
                if (UnReadCount==0)
                {
                    return "";
                }
                return UnReadCount > 99 ? "99+" : UnReadCount.ToString();
            }
        }
        public string HeadshotPathDesc
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_50") : ConfigHelper.GetPath("DefaultPath_50", HeadshotPath);
            }
        }
        #endregion
    }
}
