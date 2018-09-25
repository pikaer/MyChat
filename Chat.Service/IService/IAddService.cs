using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Model.Common.Request;
using Chat.Model.DTO.Add;
using Chat.Model.Entity.FriendInfo;

namespace Chat.Service.IService
{
    public interface IAddService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        BootstrapTableResponse<UserTableDTO> GetUserList(AddFriendRequest req,long Id);

        /// <summary>
        /// 添加好友请求
        /// </summary>
        OperationResult<bool> AddFriend(AddRequest request);

        /// <summary>
        /// 删除添加好友请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        OperationResult<bool> DeleteAddRequest(CommonRequest request);
    }
}
