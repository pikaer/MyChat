using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Model.DTO.Friend;
using Chat.Model.Entity.FriendInfo;
using System.Collections.Generic;

namespace Chat.Service.IService
{
    public interface IFriendService
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        BootstrapTableResponse<FriendDTO> GetFriendList(long userId);

        /// <summary>
        /// 获取新好友列表
        /// </summary>
        OperationResult<List<AddRequestDTO>> GetNewFriendList(long userId);

        OperationResult<bool> AddValidate(AddRequestDTO req);
    }
}
