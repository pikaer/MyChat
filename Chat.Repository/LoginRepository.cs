using System;
using Chat.Model.Common.Helper;
using Chat.Model.DTO.UserInfo;
using Dapper;
using Chat.Ultilities.Loger;
using Chat.Model.Common;

namespace Chat.Repository
{
    public class LoginRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.MyChat;
        }

        public OperationResult<UserDTO> GetUserInfoByMobile(string mobile)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<UserDTO>();
                try
                {
                    var sql = @"SELECT [Id]
                                  ,[PassWord]
                                  ,[Mobile]
                                  ,[Gender]
                                  ,[NickName]
                                  ,[TrueName]
                              FROM [dbo].[userInfo_User]
                              Where Mobile=@Mobile";
                    result.Content = Db.QueryFirstOrDefault<UserDTO>(sql, new { Mobile = mobile });
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("LoginRepository", "GetUserInfoByMobile", ex, "入参：" + mobile);
                }
                return result;
            }
        }

        /// <summary>
        /// 通过Id获取用户信息
        /// </summary>
        public UserDTO GetUserInfoById(long id)
        {
            using (var Db = GetDbConnection())
            {
                var result = new UserDTO();
                try
                {
                    var sql = @"SELECT Id
                                  ,PassWord
                                  ,Mobile
                                  ,Gender
                                  ,NickName
                                  ,TrueName
                                  ,Signature
                                  ,HomeProvinceId
                                  ,HomeCityId
                                  ,HeadshotPath
                              FROM dbo.userInfo_User
                              Where Id=@Id";
                    result = Db.QueryFirstOrDefault<UserDTO>(sql, new { Id = id });
                }
                catch (Exception ex)
                {
                    Log.Exception("LoginRepository", "GetUserInfoByMobile", ex, "入参：" + id);
                }
                return result;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public OperationResult<long> Register(UserDTO dto)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<long>();
                try
                {
                    var sql = @"INSERT INTO [dbo].[userInfo_User]
                                              ([PassWord]
                                              ,[Mobile]
                                              ,[Gender]
                                              ,[NickName]
                                              ,[TrueName]
                                              ,[CreateTime])
                                        VALUES
                                              (@PassWord
                                              ,@Mobile
                                              ,@Gender
                                              ,@NickName
                                              ,@TrueName
                                              ,@CreateTime)
                            SELECT CAST(SCOPE_IDENTITY() as bigint)";
                    result.Content = Db.QueryFirstOrDefault<long>(sql, dto);
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("LoginRepository", "Register", ex, "入参：" + dto.ToString());
                }
                return result;
            }
        }
    }
}
