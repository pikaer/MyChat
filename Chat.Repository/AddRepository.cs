using Chat.Model.Common;
using Chat.Model.Common.Helper;
using Chat.Model.Entity.FriendInfo;
using Chat.Ultilities.Loger;
using System;
using Dapper;
using Chat.Model;

namespace Chat.Repository
{
    public class AddRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.MyChat;
        }

        /// <summary>
        /// 添加好友请求
        /// </summary>
        public OperationResult<bool> AddFriend(AddRequest request)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<bool>();
                try
                {
                    var sql = @"INSERT INTO dbo.friendInfo_AddRequest
                                         (ReqUserId
                                         ,ReqOriginId
                                         ,AddType
                                         ,AddStat
                                         ,DescName
                                         ,ReqContent
                                         ,CreateTime
                                         ,UpdateTime)
                                   VALUES
                                         (@ReqUserId
                                         ,@ReqOriginId
                                         ,@AddType
                                         ,@AddStat
                                         ,@DescName
                                         ,@ReqContent
                                         ,@CreateTime
                                         ,@UpdateTime)";
                    result.Content = Db.Execute(sql, request) > 0;
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("AddRepository", "AddFriend", ex);
                }
                return result;
            }
        }

        public AddRequest GetAddDetail(long Id)
        {
            using (var Db = GetDbConnection())
            {
                var result = new AddRequest();
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
                                      ,IsUserDelete
                                      ,IsOriginDelete
                                  FROM dbo.friendInfo_AddRequest
                                  WHERE Id=@Id";
                    result = Db.QueryFirstOrDefault<AddRequest>(sql, new { Id = Id});
                }
                catch (Exception ex)
                {
                    Log.Exception("AddRepository", "GetAddDetail", ex);
                }
                return result;
            }
        }

        public bool DeleteAddRequest(AddRequest dto)
        {
            using (var Db = GetDbConnection())
            {
                try
                {
                    var sql = @"UPDATE dbo.friendInfo_AddRequest
                                   SET IsUserDelete=@IsUserDelete
                                      ,IsOriginDelete=@IsOriginDelete
                                 WHERE Id=@Id";
                    return Db.Execute(sql, dto) >0;
                }
                catch (Exception ex)
                {
                    Log.Exception("AddRepository", "GetAddDetail", ex);
                    return false;
                }
            }
        }
    }
}
