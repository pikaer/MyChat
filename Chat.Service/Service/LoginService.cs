using Chat.Service.IService;
using System;
using Chat.Model.Common.Helper;
using Chat.Model.DTO.UserInfo;
using Chat.Repository;
using Chat.Ultilities.Extensions;
using Chat.Ultilities.Loger;
using Chat.Model.Common;

namespace Chat.Service.Service
{
    public class LoginService : ILoginService
    {
        private LoginRepository loginRepository = new LoginRepository();

        public OperationResult<UserDTO> GetUserInfoById(long Id)
        {
            var rtn = new OperationResult<UserDTO>();
            try
            {
                var dto = loginRepository.GetUserInfoById(Id);
                if (dto != null)
                {
                    rtn.Content = dto;
                }
            }
            catch (Exception ex)
            {
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("LoginService", "GetUserInfoById", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 通过手机号获取当前登录用信息
        /// </summary>
        public OperationResult<UserDTO> GetUserInfoByMobile(string mobile)
        {
            return loginRepository.GetUserInfoByMobile(mobile);
        }

        /// <summary>
        /// 用户注册账户
        /// </summary>
        public OperationResult<long> Register(UserDTO dto)
        {
            dto.PassWord= Md5.GetMd5Str32(dto.PassWord);//密码32位加密
            dto.CreateTime = DateTime.Now;
            return loginRepository.Register(dto);
        }
    }
}
