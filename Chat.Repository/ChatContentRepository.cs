using Chat.Model.Common;
using Chat.Model.Common.Helper;
using System;
using System.Linq;
using Dapper;
using Chat.Model;
using Chat.Model.DTO.Chat;
using Chat.Ultilities.Loger;
using Chat.Model.Common.Response;
using System.Collections.Generic;

namespace Chat.Repository
{
    public class ChatContentRepository : BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.MyChat;
        }

        /// <summary>
        /// 向数据库插入聊天内容
        /// </summary>
        public OperationResult<bool> AddPersonalChatHistory(ChatHistoryDTO dto)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<bool>();
                try
                {
                    var sql = @"INSERT INTO [dbo].[chat_PersonalChatHistory]
                                              ([Id]
                                              ,[FriendId]
                                              ,[ChatContent]
                                              ,[Type]
                                              ,[CreateTime])
                                        VALUES
                                              (@Id
                                              ,@FriendId
                                              ,@ChatContent
                                              ,@Type
                                              ,@CreateTime)";
                    result.Content = Db.Execute(sql, dto) > 0;
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("ChatContentRepository", "AddPersonalChatHistory", ex, "入参：" + dto.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 获取聊天好友列表
        /// </summary>
        /// <param name="userId">当前登录者Id</param>
        /// <returns>BootstrapTable</returns>
        public List<ChatFriendDTO> GetChatFriendList(long userId)
        {
            using (var Db = GetDbConnection())
            {
                var query = new List<ChatFriendDTO>();
                try
                {
                    //查询主动发起的聊天
                    var sql1 = @"Select temp.ChatContent as RecentChatContent,temp.CreateTime as RecentChatTime,temp.FriendId,temp.Type,temp.DescName,temp.UserId,temp.OriginId,temp.HeadshotPath
                            From (
                                  Select row_number() over(partition by friend.OriginId order by history.CreateTime desc) as rownum  --对每个分组添加编号
                                        ,history.ChatContent
                                        ,history.CreateTime
                                        ,history.Type
                                        ,history.FriendId
                            			,friend.DescName
                                        ,friend.UserId
										,friend.OriginId
                                        ,userinfo.HeadshotPath
                                  From chat_PersonalChatHistory history
                                  Inner join friendInfo_Friend friend
                                  on friend.Id=history.FriendId
                                  Inner Join dbo.userInfo_User userinfo
                                  ON userinfo.Id=friend.UserId
                                  Where friend.IsDeleted=0 and friend.UserId=@userId
                            	  ) temp
                            Where temp.rownum=1 --取出编号为1的数据
                            ";

                    //查询被动发起的聊天
                    var sql2 = @"Select temp.ChatContent as RecentChatContent,temp.CreateTime as RecentChatTime,temp.FriendId,temp.Type,temp.DescName,temp.UserId,temp.OriginId,temp.HeadshotPath
                            From (
                                  Select row_number() over(partition by friend.UserId order by history.CreateTime desc) as rownum  --对每个分组添加编号
                                        ,history.ChatContent
                                        ,history.CreateTime
                                        ,history.Type
                                        ,history.FriendId
                            			,friend.DescName
                                        ,friend.UserId
										,friend.OriginId
                                        ,userinfo.HeadshotPath
                                  From chat_PersonalChatHistory history
                                  Inner join friendInfo_Friend friend
                                  on friend.Id=history.FriendId
                                  Inner Join dbo.userInfo_User userinfo
                                  ON userinfo.Id=friend.UserId
                                  Where friend.IsDeleted=0 and friend.OriginId=@userId
                            	  ) temp
                            Where temp.rownum=1  --取出编号为1的数据
                            ";
                    //查询主动发起的聊天
                    var list1 = Db.Query<ChatFriendDTO>(sql1, new { userId = userId });
                    //查询被动发起的聊天
                    var list2 = Db.Query<ChatFriendDTO>(sql2, new { userId = userId });
                    foreach (ChatFriendDTO dto in list1)
                    {
                        if (list2.Count(a => a.UserId == dto.OriginId) == 0)   //对方在这个时间段没有给我发过聊天
                        {
                            query.Add(dto);
                        }
                        else   //对方也给自己发了聊天
                        {
                            //判断最后一条聊天谁发的
                            var entity = list2.FirstOrDefault(a => a.UserId == dto.OriginId && a.RecentChatTime > dto.RecentChatTime);
                            if (entity != null)  //对方发的最后一条消息
                            {
                                entity.OriginId = entity.UserId.Value;  //好友的源Id为在User实体表中对应的主键
                                entity.DescName = dto.DescName;   //该好友的备注名
                                query.Add(entity);
                            }
                            else
                            {
                                query.Add(dto);
                            }
                        }
                    }
                    foreach (ChatFriendDTO dto in list2)
                    {
                        if (list1.Count(a => a.OriginId == dto.UserId) == 0)  //这段时间都是对方发来的，我没有回复过聊天
                        {
                            dto.OriginId = dto.UserId.Value; //好友的源Id为在User实体表中对应的主键
                            query.Add(dto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Exception("ChatContentRepository", "GetChatFriendList", ex, "用户Id" + userId);
                }
                return query;
            }
        }
        /// <summary>
        /// 对方发来的最新聊天内容
        /// </summary>
        public OperationResult<ChatHistoryResponse> GetChatContent(CommonRequest request)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<ChatHistoryResponse>()
                {
                    Content = new ChatHistoryResponse()
                };
                try
                {
                    var sql = @"Select ChatContent,history.CreateTime,Type,userinfo.HeadshotPath
                            from [dbo].[chat_PersonalChatHistory] history
							INNER JOIN [dbo].[friendInfo_Friend] friend
							ON friend.Id=history.FriendId
                            Inner Join dbo.userInfo_User userinfo
                            ON userinfo.Id=friend.UserId
                            Where friend.UserId=@originId and friend.OriginId=@userId and history.CreateTime>@createTime
                            Order by history.CreateTime ";
                    result.Content.ChatHistory = Db.Query<ChatHistoryDTO>(sql, new { userId = request.UserId, originId = request.OriginId, createTime = request.Time }).ToList();
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("ChatContentRepository", "GetChatContent", ex, "userId=" + request.UserId + "originId=" + request.OriginId);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取聊天历史记录
        /// </summary>
        public OperationResult<ChatHistoryResponse> GetChatContentHistory(CommonRequest request)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<ChatHistoryResponse>()
                {
                    Content = new ChatHistoryResponse()
                };
                try
                {
                    //查询主动发给对方的聊天
                    var sql1 = @" SELECT history.Id
                                  ,history.FriendId
                                  ,history.ChatContent
                                  ,history.Type
                                  ,history.CreateTime
                                  ,1 as OnRight
                                  ,userinfo.HeadshotPath
                              FROM dbo.chat_PersonalChatHistory history
							  INNER JOIN dbo.friendInfo_Friend friend
							  ON friend.Id=history.FriendId
                              Inner Join dbo.userInfo_User userinfo
                              ON userinfo.Id=friend.UserId
                              Where friend.UserId=@UserId and friend.OriginId=@OriginId";
                    //查询对方发来的聊天
                    var sql2 = @" SELECT history.Id
                                  ,history.FriendId
                                  ,history.ChatContent
                                  ,history.Type
                                  ,history.CreateTime
                                  ,0 as OnRight
                                  ,userinfo.HeadshotPath
                              FROM dbo.chat_PersonalChatHistory history
							  INNER JOIN dbo.friendInfo_Friend friend
							  ON friend.Id=history.FriendId
                              Inner Join dbo.userInfo_User userinfo
                              ON userinfo.Id=friend.UserId
                              Where friend.UserId=@OriginId and friend.OriginId=@UserId";
                    var list1 = Db.Query<ChatHistoryDTO>(sql1, new { UserId = request.UserId, OriginId = request.OriginId });
                    var list2 = Db.Query<ChatHistoryDTO>(sql2, new { UserId = request.UserId, OriginId = request.OriginId });
                    //先倒序取最新100条消息，最后按实际正序排列
                    result.Content.ChatHistory = list1.Concat(list2).OrderByDescending(a => a.CreateTime).Take(100).OrderBy(a => a.CreateTime).ToList();
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("ChatContentRepository", "GetChatContentHistory", ex, "UserId = " + request.UserId + "OriginId = " + request.OriginId);
                }
                return result;
            }
        }
        /// <summary>
        /// 刷新未读消息内容及条数
        /// </summary>
        public OperationResult<List<ChatFriendDTO>> RefrashUnReadMsg(long userId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<List<ChatFriendDTO>>();
                try
                {
                    var sql = @"Select lasttab.OriginId
		                              ,lasttab.RecentChatContent
		                              ,lasttab.RecentChatTime
		                              ,lasttab.Type
		                              ,lasttab.ReadTime
		                              ,unread.UnReadCount
		                        From 
                                    (Select IsNull(tab1.OriginId,tab2.OriginId) as OriginId
									      --以下通过比较tab2和tab1的RecentChatTime大小确定取哪一个聊天记录
									      ,CASE when tab1.RecentChatTime> =tab2.RecentChatTime THEN tab1.RecentChatContent else tab2.RecentChatContent end as RecentChatContent
										  ,CASE when tab1.RecentChatTime> =tab2.RecentChatTime THEN tab1.RecentChatTime else tab2.RecentChatTime end as RecentChatTime
										  ,CASE when tab1.RecentChatTime> =tab2.RecentChatTime THEN tab1.Type else tab2.Type end as Type
										  ,CASE when tab1.RecentChatTime> =tab2.RecentChatTime THEN tab1.ReadTime else tab2.ReadTime end as ReadTime
								    From(
		                            --查询对方发给我的最后一条消息
		                            Select temp.UserId as OriginId,temp.RecentChatContent,temp.CreateTime as RecentChatTime,temp.Type,temp.ReadTime
					        		From (Select row_number() over(partition by friend.UserId order by history.CreateTime desc) as rownum  --对每个分组添加编号
                                                ,history.ChatContent as RecentChatContent
                                                ,history.CreateTime
                                                ,history.Type
					        					,friend.UserId
												,IsNull(friend.ReadTime,history.CreateTime) as ReadTime
					        					,friend.DeleteChatContentTime
                                          From chat_PersonalChatHistory history
                                          Inner join friendInfo_Friend friend
                                          on friend.Id=history.FriendId
                                          Where friend.IsDeleted=0 and friend.OriginId=@UserId
                                    	  ) temp
					        	    Where temp.rownum=1
									) tab1
									--全连接,获取自己和对方发的最后一条消息
									Full join (  
									--查询我发给对方的最后一条消息
									Select temp.OriginId as OriginId,temp.RecentChatContent,temp.CreateTime as RecentChatTime,temp.Type,temp.ReadTime
					        		From (Select row_number() over(partition by friend.OriginId order by history.CreateTime desc) as rownum  --对每个分组添加编号
                                                ,history.ChatContent as RecentChatContent
                                                ,history.CreateTime
                                                ,history.Type
					        					,friend.OriginId
												,IsNull(friend.ReadTime,history.CreateTime) as ReadTime
					        					,friend.DeleteChatContentTime
                                          From chat_PersonalChatHistory history
                                          Inner join friendInfo_Friend friend
                                          on friend.Id=history.FriendId
                                          Where friend.IsDeleted=0 and friend.UserId=@UserId
                                    	  ) temp
					        	    Where temp.rownum=1 
									) tab2
									on tab2.OriginId=tab1.OriginId
									) lasttab
							  --获取未读条数
							  Left join (Select temp.UserId as OriginId,Count(temp.UserId) as UnReadCount
						           	From (Select friend.UserId
                                             From chat_PersonalChatHistory history
                                             Inner join friendInfo_Friend friend
                                             on friend.Id=history.FriendId
                                             Where friend.IsDeleted=0 and friend.OriginId=@UserId and IsNull(friend.ReadTime,history.CreateTime)<history.CreateTime
                                       	  )temp
						           	Group by temp.UserId
								   ) unread
					          ON unread.OriginId=lasttab.OriginId";
                    result.Content = Db.Query<ChatFriendDTO>(sql, new { UserId = userId }).ToList();
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("ChatContentRepository", "RefrashUnReadeMsg", ex, "userId=" + userId);
                }
                return result;
            }
        }
        /// <summary>
        /// 将未读标记为已读
        /// </summary>
        public OperationResult<bool> UpdateReadTime(CommonRequest request)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<bool>();
                try
                {
                    var sql = @"UPDATE [dbo].[friendInfo_Friend]
                               SET [UpdateTime] = @now ,[ReadTime] = @now
                             WHERE UserId=@OriginId and OriginId=@UserId";
                    result.Content = Db.Execute(sql, new { OriginId = request.OriginId, UserId = request.UserId, now = DateTime.Now }) > 0;
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("ChatContentRepository", "UpdateReadTime", ex, "UserId=" + request.UserId + "OriginId=" + request.OriginId);
                }
                return result;
            }
        }

        /// <summary>
        /// 判断是否删除聊天列表
        /// </summary>
        /// <returns></returns>
        public DateTime DeleteChatContentTime(long userId,long origionId)
        {
            using (var Db = GetDbConnection())
            {
                var time = new DateTime();
                try
                {
                    var sql = @"SELECT DeleteChatContentTime
                                From dbo.friendInfo_Friend 
                                WHERE UserId=@UserId AND OriginId=@OriginId";
                    time = Db.QueryFirstOrDefault<DateTime>(sql, new { userId = userId, OriginId = origionId });
                }
                catch (Exception ex)
                {
                    Log.Exception("ChatContentRepository", "DeleteChatContentTime", ex);
                }
                return time;
            }
        }
    }
}
