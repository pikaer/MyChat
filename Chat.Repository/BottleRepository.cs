using Chat.Model.DTO.Discovery;
using Chat.Model.Entity.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Chat.Ultilities.Loger;
using Chat.Model.Common.Enum;

namespace Chat.Repository
{
    public class BottleRepository:BaseRepository
    {
        protected override DbEnum GetDbEnum()
        {
            return DbEnum.MyChat;
        }

        /// <summary>
        /// 获取一个新的漂流瓶
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Bottle NewBottle(long userId)
        {
            var bottle = new Bottle();
            try
            {
                using (var Db = GetDbConnection())
                {
                    var sql = @"SELECT top 1
                                       Id
                                      ,UserId
                                      ,BottleState
                                      ,BottleContent
                                      ,CreateTime
                                      ,UpdateTime
                                  FROM dbo.discovery_Bottle
                                  Where (BottleState=0 or BottleState=4) and UserId != @Id
                                  Order by UpdateTime";
                    bottle = Db.QueryFirstOrDefault<Bottle>(sql,new { Id= userId });
                }
            }
            catch (Exception ex)
            {
                Log.Exception("DiscoveryRepository", "NewBottle", ex);
            }
            return bottle;
        }

        /// <summary>
        /// 插入漂流瓶数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool InsertBottleData(Bottle request)
        {
            try
            {
                using (var Db = GetDbConnection())
                {
                    var sql = @"INSERT INTO dbo.discovery_Bottle
                                                 (UserId
                                                 ,BottleState
                                                 ,BottleContent
                                                 ,CreateTime
                                                 ,UpdateTime)
                                           VALUES
                                                 (@UserId
                                                 ,@BottleState
                                                 ,@BottleContent
                                                 ,@CreateTime
                                                 ,@UpdateTime)";
                    return Db.Execute(sql, request)>0;
                }
            }
            catch (Exception ex)
            {
                Log.Exception("DiscoveryRepository", "InsertBottleData", ex);
                return false;
            }
        }

        /// <summary>
        /// 所有我捡到过的未回复的漂流瓶
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<BottleListDTO> AllBottleList(long userId)
        {
            var bottle = new List<BottleListDTO>();
            try
            {
                using (var Db = GetDbConnection())
                {
                   var sql = @"SELECT bottle.Id AS BottleId
                                      ,bottle.UserId
                                      ,BottleState
                                      ,BottleContent
                                      ,bottle.CreateTime
                                	  ,ISNULL(city.CityName,'远方') AS CityName
                                	  ,userinfo.NickName
                                	  ,userinfo.Gender
                                  FROM dbo.discovery_Bottle bottle
                                  INNER JOIN dbo.userInfo_User userinfo
                                  On userinfo.Id=bottle.UserId
                                  LEFT JOIN dbo.provinces_City city
                                  ON userinfo.HomeCityId=city.CityId
                                  LEFT JOIN dbo.discovery_BottleChat chat
                                  ON chat.BottleId=bottle.Id
                                  WHERE bottle.OriginId=@Id AND chat.Id IS null and bottle.BottleState=1
                                  Order by bottle.UpdateTime desc ";
                    bottle = Db.Query<BottleListDTO>(sql,new { Id= userId }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Exception("DiscoveryRepository", "MyBottleList", ex);
            }
            return bottle;
        }

        /// <summary>
        /// 我扔出去的漂流瓶列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MyBottleDTO> MyThrowBottleList(long userId)
        {
            var bottle = new List<MyBottleDTO>();
            try
            {
                using (var Db = GetDbConnection())
                {
                    var sql = @"  SELECT bottle.Id AS BottleId
								  ,bottle.UserId
                                  ,bottle.OriginId
								  ,bottle.BottleState
								  ,tempchat.BottleChatContent AS RecentContent
                                  ,tempchat.CreateTime as LastTime
								  ,userinfo.NickName
                                  ,ISNULL(city.CityName,'远方') AS CityName
								  ,userinfo.HeadshotPath
								  FROM dbo.discovery_Bottle bottle
								  Inner JOIN (
                                       	      SELECT temp.BottleId,temp.CreateTime,temp.BottleChatContent
                                       		   FROM (
                                       	            SELECT row_number() over(partition by chat.BottleId order by chat.CreateTime desc) as rownum  --对每个分组添加编号
                                       				 ,chat.BottleId
                                       				 ,chat.CreateTime
                                       				 ,chat.BottleChatContent
                                       				 FROM dbo.discovery_BottleChat chat
                                                            ) temp
                                       		  WHERE temp.rownum=1
                                       	    ) tempchat
                                            ON tempchat.BottleId=bottle.Id
								  INNER JOIN dbo.userInfo_User userinfo
                                  On userinfo.Id=bottle.OriginId
                                  LEFT JOIN dbo.provinces_City city
                                  ON userinfo.HomeCityId=city.CityId
                                  Where bottle.UserId=@Id and bottle.BottleState!=2";
                    bottle = Db.Query<MyBottleDTO>(sql,new { Id= userId }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Exception("DiscoveryRepository", "MyThrowBottleList", ex);
            }
            return bottle;
        }

        /// <summary>
        /// 我回复过的漂流瓶
        /// </summary>
        public List<MyBottleDTO> MyReceivedBottle(long userId)
        {
            var bottle = new List<MyBottleDTO>();
            try
            {
                using (var Db = GetDbConnection())
                {
                    var sql = @" SELECT bottle.Id AS BottleId
                                       ,bottle.UserId
                                       ,bottle.OriginId
                                       ,BottleState
                                       ,tempchat.BottleChatContent AS RecentContent
                                       ,tempchat.CreateTime as LastTime
                                       ,ISNULL(city.CityName,'远方') AS CityName
                                 	   ,userinfo.NickName
                                 	   ,userinfo.HeadshotPath
                                   FROM dbo.discovery_Bottle bottle
                                   INNER JOIN  dbo.userInfo_User userinfo
                                   On userinfo.Id=bottle.UserId
                                   LEFT JOIN dbo.provinces_City city
                                   ON userinfo.HomeCityId=city.CityId
                                   INNER JOIN (
                                 	      SELECT temp.BottleId,temp.CreateTime,temp.BottleChatContent
                                 		   FROM (
                                 	            SELECT row_number() over(partition by chat.BottleId order by chat.CreateTime desc) as rownum  --对每个分组添加编号
                                 				 ,chat.BottleId
                                 				 ,chat.CreateTime
                                 				 ,chat.BottleChatContent
                                 				 FROM dbo.discovery_BottleChat chat
                                                      ) temp
                                 		  WHERE temp.rownum=1
                                 	    ) tempchat
                                   ON tempchat.BottleId=bottle.Id
                                   Where bottle.OriginId = @Id and bottle.BottleState!=3 ";
                    bottle = Db.Query<MyBottleDTO>(sql, new { Id = userId }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Exception("DiscoveryRepository", "MyThrowBottleList", ex);
            }
            return bottle;
        }

        /// <summary>
        /// 删除漂流瓶
        /// </summary>
        public bool DeleteBottle(BottleStateEnum state, long Id)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"Update dbo.discovery_Bottle
                            Set BottleState=@BottleState
                                ,UpdateTime=@now
                            Where Id=@Id";
                return Db.Execute(sql, new { BottleState = state ,now=DateTime.Now, Id = Id }) > 0;
            }
        }

        /// <summary>
        /// 插入瓶子回复内容
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public bool InsertBottleChat(BottleChat chat)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"INSERT INTO dbo.discovery_BottleChat
                                           (BottleId
                                           ,UserId
                                           ,OriginId
                                           ,BottleChatContent
                                           ,ChatState
                                           ,CreateTime
                                           ,UpdateTime)
                                     VALUES
                                           (@BottleId
                                           ,@UserId
                                           ,@OriginId
                                           ,@BottleChatContent
                                           ,@ChatState
                                           ,@CreateTime
                                           ,@UpdateTime)";
                return Db.Execute(sql, chat) > 0;
            }
        }

        /// <summary>
        /// 删除漂流瓶对话
        /// </summary>
        public bool DeleteBottleChat(BottleChatStateEnum state, long chatId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.discovery_BottleChat
                               SET ChatState= @State
                                  ,UpdateTime = @UpdateTime
                             WHERE Id=@Id";
                return Db.Execute(sql, new { State = state, UpdateTime = DateTime.Now, Id = chatId}) > 0;
            }
        }

        /// <summary>
        /// 更新漂流瓶状态
        /// </summary>
        public bool UpdateBottle(BottleStateEnum state,long bottleId,long originId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"UPDATE dbo.discovery_Bottle
                               SET BottleState= @BottleState
                                  ,OriginId = @OriginId
                                  ,UpdateTime = @UpdateTime
                             WHERE Id=@Id";
                return Db.Execute(sql, new { BottleState= state , UpdateTime =DateTime.Now,Id= bottleId,OriginId= originId }) > 0;
            }
        }
        
        /// <summary>
        /// 某一个瓶子详情
        /// </summary>
        public Bottle GetBottlebyId(long bottleId)
        {
            using (var Db = GetDbConnection())
            {
                var sql = @"SELECT Id
                                  ,UserId
                                  ,OriginId
                                  ,BottleState
                                  ,BottleContent
                                  ,CreateTime
                                  ,UpdateTime
                              FROM dbo.discovery_Bottle
                              WHERE Id=@Id";
                return Db.QueryFirstOrDefault<Bottle>(sql, new { Id = bottleId });
            }
        }

        /// <summary>
        /// 某一个瓶子回复列表
        /// </summary>
        /// <param name="bottleId">瓶子Id</param>
        public List<BottleChat> GetBottleChatById(long bottleId)
        {
            using(var Db=GetDbConnection())
            {
                var sql = @"SELECT Id
                                  ,BottleId
                                  ,UserId
                                  ,OriginId
                                  ,ChatState
                                  ,BottleChatContent
                                  ,CreateTime
                                  ,UpdateTime
                              FROM dbo.discovery_BottleChat
                              WHERE BottleId=@Id";
                return Db.Query<BottleChat>(sql,new { Id= bottleId }).ToList();
            }
        }
    }
}
