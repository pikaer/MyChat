using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Model.Common.Response;
using Chat.Model.DTO.Chat;
using System.Collections.Generic;

namespace Chat.Service.IService
{
    public interface IChatContentService
    {
        /// <summary>
        /// 向数据库插入聊天内容
        /// </summary>
        /// <param name="dto">聊天内容</param>
        /// <param name="userId">当前登录者Id</param>
        OperationResult<bool> AddPersonalChatHistory(ChatHistoryDTO dto,long userId);

        /// <summary>
        /// 获取聊天好友列表
        /// </summary>
        /// <param name="userId">当前登录者Id</param>
        BootstrapTableResponse<ChatFriendDTO> GetChatFriendList(long userId);

        /// <summary>
        /// 对方发来的最新聊天内容
        /// </summary>
        OperationResult<ChatHistoryResponse> GetChatContent(CommonRequest request);

        /// <summary>
        /// 获取聊天历史记录
        /// </summary>
        OperationResult<ChatHistoryResponse> GetChatContentHistory(CommonRequest request);

        /// <summary>
        /// 刷新未读消息条数
        /// </summary>
        OperationResult<List<ChatFriendDTO>> RefrashUnReadMsg(long userId);

        /// <summary>
        /// 将未读标记为已读
        /// </summary>
        OperationResult<bool> UpdateReadTime(CommonRequest request);
    }
}
