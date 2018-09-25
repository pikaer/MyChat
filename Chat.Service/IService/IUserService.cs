using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Model.Common.Request;
using Chat.Model.DTO.UserInfo;
using System.Collections.Generic;

namespace Chat.Service.IService
{
    public interface IUserService
    {
        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        OperationResult<List<User>> GetUserList();

        /// <summary>
        /// 用户详情
        /// </summary>
        OperationResult<UserDTO> UserDetail(CommonRequest request);

        /// <summary>
        /// 删除好友
        /// </summary>
        OperationResult<bool> DeleteFriend(CommonRequest request);

        /// <summary>
        /// 上传头像
        /// </summary>
        OperationResult<bool> UpLoadHeadPhoto(UpLoadPhotoRequest req,long userId,string path);

        /// <summary>
        /// 获取用户详情页数据
        /// </summary>
        OperationResult<BasicInfoDTO> GetBasicInfo(long userId);

        /// <summary>
        /// 编辑用户详情
        /// </summary>
        OperationResult<bool> BasicInfoEdit(BasicInfoDTO request);
        
    }
}
