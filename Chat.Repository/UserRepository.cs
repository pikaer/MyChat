using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Chat.Model.Common.Helper;
using Chat.Model.Common;
using Chat.Model.DTO.UserInfo;
using Chat.Model.Common.Request;
using Chat.Ultilities.Loger;

namespace Chat.Repository
{
    public class UserRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.MyChat;
        }
        public OperationResult<List<User>> GetUserList()
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<List<User>>();
                var list = new List<User>();
                try
                {
                    var sql = @"select Id,Gender,NickName,TrueName,CreateTime From [dbo].[userInfo_User] Where IsDeleted=0";
                    list = Db.Query<User>(sql).ToList();
                }
                catch(Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    Log.Exception("UserRepository", "GetUserList", ex);
                }
                result.Content = list;
                return result;
            }
        }

        public UserDTO UserDetail(CommonRequest request)
        {
            using (var Db = GetDbConnection())
            {
                var result = new UserDTO();
                try
                {
                    var sql = @"SELECT AddType
                                      ,DescName
                                  FROM dbo.friendInfo_Friend
                                  WHERE UserId=@UserId and OriginId=@OriginId";
                    result = Db.QueryFirstOrDefault<UserDTO>(sql,new { UserId= request.UserId , OriginId = request.OriginId });
                }
                catch(Exception ex)
                {
                    Log.Exception("UserRepository", "UserDetail", ex);
                }
                return result;
            }
        }

        public List<UserDTO> UserListByParam(AddFriendRequest req, long Id)
        {
            using (var Db = GetDbConnection())
            {
                var list = new List<UserDTO>();
                try
                {
                    var sql = @"select Id,Gender,NickName,TrueName,CreateTime,HeadshotPath From [dbo].[userInfo_User] Where IsDeleted=0 and Id !="+Id;
                    if(req.HomeProvinceId>0)
                    {
                        sql += " And HomeProvinceId=@HomeProvinceId ";
                    }
                    if (req.HomeCityId > 0)
                    {
                        sql += " And HomeCityId=@HomeCityId ";
                    }
                    if (req.HomeAreaId > 0)
                    {
                        sql += " And HomeAreaId=@HomeAreaId ";
                    }
                    if (req.HomeTownProvinceId > 0)
                    {
                        sql += " And HomeTownProvinceId=@HomeTownProvinceId ";
                    }
                    if (req.HomeTownCityId > 0)
                    {
                        sql += " And HomeTownCityId=@HomeTownCityId ";
                    }
                    if (req.HomeTownAreaId > 0)
                    {
                        sql += " And HomeTownAreaId=@HomeTownAreaId ";
                    }
                    sql += "Order By CreateTime desc  OFFSET 20*(@PageIndex-1)  ROWS FETCH NEXT 20 ROWS ONLY";
                    list = Db.Query<UserDTO>(sql, req).ToList();
                }
                catch(Exception ex)
                {
                    Log.Exception("UserRepository", "UserListByParam", ex);
                }
                return list;
            }
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        public OperationResult<bool> DeleteFriend(CommonRequest request)
        {
            using (var Db = GetDbConnection())
            {
                var rtn = new OperationResult<bool>();
                try
                {
                    var sql = @"DELETE FROM dbo.friendInfo_Friend WHERE UserId=@UserId AND OriginId=@OriginId 
                                DELETE FROM dbo.friendInfo_Friend WHERE UserId=@OriginId AND OriginId=@UserId";
                    rtn.Content=Db.Execute(sql, new { UserId = request.UserId, OriginId = request.OriginId })>0;
                }
                catch (Exception ex)
                {
                    Log.Exception("UserRepository", "DeleteFriend", ex);
                    rtn.Content = false;
                    rtn.ResultType = OperationResultType.Error;
                }
                return rtn;
            }
        }

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateHeadPath(string path,long id)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Update dbo.userInfo_User
                            set HeadshotPath=@Path
                            ,UpdateTime=@now
                            Where Id=@Id";
                return Db.Execute(sql, new { Path = path, now=DateTime.Now,Id=id }) > 0;
            }
        }

        public BasicInfoDTO GetUserById(long userId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT Id
                                  ,Gender
                                  ,NickName
                                  ,TrueName
                                  ,BirthDay
                                  ,HeadshotPath
                                  ,CreateTime
                                  ,UpdateTime
                                  ,IsDeleted
                                  ,PassWord
                                  ,Mobile
                                  ,HomeAreaId
                                  ,HomeCityId
                                  ,HomeProvinceId
                                  ,HomeTownAreaId
                                  ,HomeTownCityId
                                  ,HomeTownProvinceId
                                  ,Signature
                                  ,BloodType
                                  ,Email
                                  ,QQ
                              FROM dbo.userInfo_User
                              WHERE Id=@UserId";
                return Db.QueryFirstOrDefault<BasicInfoDTO>(sql,new { UserId =userId});
            }
        }

        /// <summary>
        /// 编辑用户详情
        public bool BasicInfoEdit(BasicInfoDTO request)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.userInfo_User
                               SET Gender = @Gender
                                  ,NickName = @NickName
                                  ,TrueName = @TrueName
                                  ,BirthDay = @BirthDay
                                  ,UpdateTime = @UpdateTime
                                  ,HomeAreaId =@HomeAreaId
                                  ,HomeCityId = @HomeCityId
                                  ,HomeProvinceId = @HomeProvinceId
                                  ,HomeTownAreaId = @HomeTownAreaId
                                  ,HomeTownCityId = @HomeTownCityId
                                  ,HomeTownProvinceId = @HomeTownProvinceId
                                  ,Signature = @Signature
                                  ,BloodType = @BloodType
                                  ,Email = @Email
                                  ,QQ =@QQ
                             WHERE Id=@Id";
                return Db.Execute(sql, request) > 0;
            }
        }

        public UserDTO GetUserDefaultInfo(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
