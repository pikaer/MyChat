using Chat.Model.Common.Helper;
using Chat.Model.DTO.UserInfo;

namespace Chat.Service.IService
{
    public interface ILoginService
    {
        /// <summary>
        /// 通过手机号获取当前登录用户信息
        /// </summary>
        OperationResult<UserDTO> GetUserInfoByMobile(string mobile);

        /// <summary>
        /// 通过Id获取当前登录用户信息
        /// </summary>
        OperationResult<UserDTO> GetUserInfoById(long Id);

        /// <summary>
        /// 用户注册账户
        /// </summary>
        OperationResult<long> Register(UserDTO dto);
    }
}
