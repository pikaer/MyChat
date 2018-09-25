using Chat.Model.DTO.Chat;
using System;
using System.Collections.Generic;

namespace Chat.Model.Common.Response
{
    /// <summary>
    /// 获取最新对方发来消息响应体
    /// </summary>
    public class ChatHistoryResponse
    {
        public ChatHistoryResponse()
        {
            ChatHistory = new List<ChatHistoryDTO>();
        }
        /// <summary>
        /// 最新获取时间
        /// </summary>
        public DateTime LastTime { get; set; }
        
        /// <summary>
        /// 将时间转化为字符串，防止前端转化后产生误差
        /// </summary>
        public string LastTimeStr
        {
            get
            {
                return LastTime.ToString("O");
            }
        }
        /// <summary>
        /// 聊天记录
        /// </summary>
        public List<ChatHistoryDTO> ChatHistory { get; set; }
    }
}
