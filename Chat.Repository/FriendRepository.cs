using System;
using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Model.Common;
using Chat.Ultilities.Loger;
using Dapper;
using System.Linq;
using Chat.Model.DTO.Friend;
using Chat.Model.Entity.FriendInfo;
using System.Collections.Generic;
using Chat.Model.Common.Enum;

namespace Chat.Repository
{
    public class FriendRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.MyChat;
        }
        public AddRequest AddRequest(long Id)
        {
            using (var Db = GetDbConnection())
            {
                var rtn = new AddRequest();
                try
                {
                    var sql = @"SELECT Id
                                   ,ReqUserId
                                   ,ReqOriginId
                                   ,AddType
                                   ,AddStat
                                   ,ReqContent
                                   ,CreateTime
                                   ,DescName
                                   ,UpdateTime
                               FROM dbo.friendInfo_AddRequest
                               WHERE Id=@Id";
                    rtn = Db.QueryFirstOrDefault<AddRequest>(sql, new { Id = Id });
                }
                catch (Exception ex)
                {
                    Log.Exception("FriendRepository", "GetFriendDetail", ex);
                }
                return rtn;
            }
        }
        public bool AddValidate(AddRequestDTO req)
        {
            using (var Db = GetDbConnection())
            {
                try
                {
                    var sql = @"Update dbo.friendInfo_AddRequest
                                Set AddStat=@AddStat
                                    ,UpdateTime=@now
                                Where Id=@Id";
                    return Db.Execute(sql, new { AddStat = req.AddStat, Id = req.Id,now=DateTime.Now})>0;
                }
                catch (Exception ex)
                {
                    Log.Exception("FriendRepository", "GetFriendDetail", ex);
                    return false;
                }
            }
        }
        public bool AddFriend(long userId,long origionId, AddTypeEnum type,string desc="")
        {
            using (var Db = GetDbConnection())
            {
                try
                {
                    var sql = @"INSERT INTO dbo.friendInfo_Friend
                                                (UserId
                                                ,OriginId
                                                ,AddType
                                                ,CreateTime
                                                ,IsDeleted
                                                ,UpdateTime
                                                ,DescName
                                                ,DeleteChatContentTime
                                                ,ReadTime)
                                          VALUES
                                                (@UserId
                                                ,@OriginId
                                                ,@AddType
                                                ,@now
                                                ,0
                                                ,@now
                                                ,@DescName
                                                ,null
                                                ,null)";
                    return Db.Execute(sql, new { UserId = userId, OriginId = origionId, now = DateTime.Now , AddType = type , DescName = desc }) > 0;
                }
                catch (Exception ex)
                {
                    Log.Exception("FriendRepository", "GetFriendDetail", ex);
                    return false;
                }
            }
        }
        /// <summary>
        /// 获取好友详情数据
        /// </summary>
        public OperationResult<Friend> GetFriendDetail(long userId, long originId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<Friend>();
                try
                {
                    var sql = @"Select Id,UserId,OriginId,AddType,CreateTime,UpdateTime,DescName,DeleteChatContentTime
                            From [dbo].[friendInfo_Friend]
                            Where IsDeleted=0 and UserId=@UserId and OriginId=@OriginId";
                    result.Content = Db.QueryFirstOrDefault<Friend>(sql, new { UserId = userId, OriginId = originId });
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("FriendRepository", "GetFriendDetail", ex, "入参：UserId=" + userId + "OriginId=" + originId);
                }
                return result;
            }
        }

        public BootstrapTableResponse<FriendDTO> GetFriendList(long userId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new BootstrapTableResponse<FriendDTO>();
                try
                {
                    var sql1 = @"SELECT userinfo.Id
                                  ,userinfo.Mobile
                                  ,userinfo.Gender
                            	  ,userinfo.NickName
                            	  ,userinfo.TrueName
                            	  ,userinfo.BirthDay
                            	  ,userinfo.HeadshotPath
                                  ,Isnull(friend.DescName,userinfo.NickName) as DescName
                              FROM [dbo].[friendInfo_Friend] friend
                              INNER JOIN [dbo].[userInfo_User] userinfo
                              ON userinfo.Id=friend.OriginId
                              WHERE friend.IsDeleted=0 AND friend.UserId=@UserId";
                    result.rows = Db.Query<FriendDTO>(sql1, new { UserId = userId }).OrderBy(a => a.NickName).ToList();
                }
                catch (Exception ex)
                {
                    Log.Exception("FriendRepository", "GetFriendList", ex, "用户Id" + userId);
                }
                return result;
            }
        }

        public OperationResult<List<AddRequestDTO>> GetNewFriendList(long userId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<List<AddRequestDTO>>();
                try
                {
                    var sql1 = @"Select req.ReqOriginId,req.IsPassive,req.Id,req.AddType,req.AddStat,req.ReqContent,req.CreateTime,req.DescName
                                    ,userinfo.Gender,userinfo.BirthDay,userinfo.HeadshotPath,userinfo.NickName
                              From(
                                    SELECT (case when ReqUserId=@UserId then 0 else 1 end) as IsPassive
                              	       ,(case when ReqUserId=@UserId then ReqOriginId else ReqUserId end) as ReqOriginId
                                         ,Id
                                         ,AddType
                                         ,AddStat
                                         ,ReqContent
                                         ,CreateTime
                                         ,DescName
                                    FROM dbo.friendInfo_AddRequest 
                                    Where (ReqOriginId=@UserId and IsOriginDelete=0) or (ReqUserId=@UserId and IsUserDelete=0)
                              ) req
                              Inner join dbo.userInfo_User userinfo
                              on req.ReqOriginId=userinfo.Id
                              Order by req.CreateTime desc";
                    result.Content = Db.Query<AddRequestDTO>(sql1, new { UserId = userId }).ToList();
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("FriendRepository", "GetNewFriendList", ex, "用户Id" + userId);
                }
                return result;
            }
        }
    }
}
