using Chat.Model;
using Chat.Model.Common;
using Chat.Model.Common.Helper;
using Chat.Model.Common.Response;
using Chat.Model.DTO.Chat;
using Chat.Repository;
using Chat.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Service.Service
{
    public class ChatContentService : IChatContentService
    {
        private ChatContentRepository chatContentRepository = new ChatContentRepository();
        private FriendRepository friendRepository = new FriendRepository();

        /// <summary>
        /// 向数据库插入聊天内容
        /// </summary>
        /// <returns></returns>
        public OperationResult<bool> AddPersonalChatHistory(ChatHistoryDTO dto,long userId)
        {
            var rtn = new OperationResult<bool>();
            dto.Id = Guid.NewGuid();
            dto.CreateTime = DateTime.Now;
            var friendDetail = friendRepository.GetFriendDetail(userId,dto.OriginId);
            if(friendDetail.Content!=null)
            {
                dto.FriendId = friendDetail.Content.Id;
                rtn =chatContentRepository.AddPersonalChatHistory(dto);
            }
            else
            {
                rtn.Message = "获取好友数据失败";
                rtn.ResultType = OperationResultType.Error;
                rtn.Content = false;
            }
            return rtn;
        }

        /// <summary>
        /// 获取聊天好友列表
        /// </summary>
        /// <param name="userId">当前登录者Id</param>
        /// <returns>BootstrapTable</returns>
        public BootstrapTableResponse<ChatFriendDTO> GetChatFriendList(long userId)
        {
            var rtn = new BootstrapTableResponse<ChatFriendDTO>();
            var list=chatContentRepository.GetChatFriendList(userId);
            foreach(ChatFriendDTO dto in list)
            {
                var origionId = dto.UserId == userId ? dto.OriginId : dto.UserId;
                var time = chatContentRepository.DeleteChatContentTime(userId, origionId.Value);
                if (dto.RecentChatTime< time)
                {
                    list.Remove(dto);
                }
            }
            rtn.rows = list.OrderByDescending(a => a.RecentChatTime.Value).ToList();
            rtn.total = list.Count;
            return rtn;
        }

        /// <summary>
        /// 对方发来的最新聊天内容
        /// </summary>
        public OperationResult<ChatHistoryResponse> GetChatContent(CommonRequest request)
        {
            var reponse= chatContentRepository.GetChatContent(request);
            reponse.Content.LastTime = DateTime.Now;
            return reponse;
        }

        /// <summary>
        /// 获取聊天历史记录
        /// </summary>
        public OperationResult<ChatHistoryResponse> GetChatContentHistory(CommonRequest request)
        {
            var list = new List<ChatHistoryDTO>();
            var reponse= chatContentRepository.GetChatContentHistory(request);
            if(reponse.ResultType==OperationResultType.Success)
            {
                list = reponse.Content.ChatHistory;
                reponse.Content.LastTime = list.Count == 0 ? DateTime.Now : list.OrderByDescending(a => a.CreateTime).FirstOrDefault().CreateTime;
            }
            return reponse;
        }

        /// <summary>
        /// 刷新未读消息条数
        /// </summary>
        public OperationResult<List<ChatFriendDTO>> RefrashUnReadMsg(long userId)
        {
            return chatContentRepository.RefrashUnReadMsg(userId);
        }

        /// <summary>
        /// 将未读标记为已读
        /// </summary>
        public OperationResult<bool> UpdateReadTime(CommonRequest request)
        {
            return chatContentRepository.UpdateReadTime(request);
        }
    }
}
