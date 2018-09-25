using Chat.Model.Common;
using Chat.Model.Common.Enum;
using Chat.Model.Common.Helper;
using Chat.Model.DTO.Discovery;
using Chat.Model.Entity.Discovery;
using Chat.Repository;
using Chat.Service.IService;
using Chat.Ultilities.Loger;
using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Model;

namespace Chat.Service.Service
{
    /// <summary>
    /// 漂流瓶功能服务
    /// </summary>
    public class BottleService :IBottleService
    {
        private BottleRepository discoveryRepository = new BottleRepository();
        private LoginRepository loginRepository = new LoginRepository();
        private CommonRepository commonRepository = new CommonRepository();
        private static string[] cityName = new string[] { "县", "市辖区", "市", "省直辖行政单位" };
        /// <summary>
        /// 扔一个瓶子
        /// </summary>
        public OperationResult<bool> ThrowOneBottle(Bottle request)
        {
            var rtn = new OperationResult<bool>();
            try
            {
                request.CreateTime = DateTime.Now;
                request.UpdateTime = DateTime.Now;
                request.BottleState = BottleStateEnum.Default;
                request.BottleContent = request.BottleContent.Trim();
                rtn.Content = discoveryRepository.InsertBottleData(request);
            }
            catch(Exception ex)
            {
                Log.Exception("DiscoveryService", "ThrowOneBottle", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 捡一个瓶子
        /// </summary>
        public OperationResult<BottleDTO> NewBottle(long userId)
        {
            var rtn = new OperationResult<BottleDTO>();
            var bottle = discoveryRepository.NewBottle(userId);
            if(bottle!=null)
            {
                var bottleDTO = new BottleDTO();
                bottleDTO.Id = bottle.Id;
                bottleDTO.UserId = bottle.UserId;
                bottleDTO.BottleState = bottle.BottleState;
                bottleDTO.BottleContent = bottle.BottleContent;
                bottleDTO.CreateTime = bottle.CreateTime;
                bottleDTO.UpdateTime = bottle.UpdateTime;
                var bottleUser = loginRepository.GetUserInfoById(bottle.UserId);
                if(bottleUser!=null)
                {
                    bottleDTO.Gender = bottleUser.Gender;
                    bottleDTO.NickName = bottleUser.NickName;
                    //城市信息
                    var city = bottleUser.HomeCityId.HasValue ? commonRepository.GetCityById(bottleUser.HomeCityId.Value) : null;
                    bottleDTO.HomeCityName = city == null ? "远方" : city.CityName;
                }
                //修改瓶子状态
                discoveryRepository.UpdateBottle(BottleStateEnum.Received, bottle.Id, userId);
                rtn.Content = bottleDTO;
            }
            return rtn;
        }

        /// <summary>
        /// 所有我捡到过的未回复的漂流瓶
        /// </summary>
        public OperationResult<List<BottleListDTO>> AllBottleList(long userId)
        {
            var rtn = new OperationResult<List<BottleListDTO>>();
            try
            {
                rtn.Content = discoveryRepository.AllBottleList(userId); 
            }
            catch(Exception ex)
            {
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("DiscoveryService", "AllBottleList", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 我的回复过的和我扔出去且被回复的瓶子列表
        /// </summary>
        public OperationResult<List<MyBottleDTO>> AllReplyBottleList(long userId)
        {
            var rtn = new OperationResult<List<MyBottleDTO>>();
            try
            {
                //我扔出去的且被回复的瓶子
                var list1= discoveryRepository.MyThrowBottleList(userId);
                //我回复过的瓶子
                var list2= discoveryRepository.MyReceivedBottle(userId);
                rtn.Content = list1.Concat(list2).OrderByDescending(a => a.LastTime).ToList();
            }
            catch (Exception ex)
            {
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("DiscoveryService", "MyBottleList", ex);
            }
            return rtn;
        }
        
        /// <summary>
        /// 某一个瓶子回复列表
        /// </summary>
        public OperationResult<List<MyBottleDTO>> ReplyBottleList(CommonRequest request)
        {
            var rtn = new OperationResult<List<MyBottleDTO>>();
            try
            {
                var list = new List<MyBottleDTO>();
                var bottle = discoveryRepository.GetBottlebyId(request.Id.Value);
                //判断是否从对话列表删除
                bool IsUser = (bottle.UserId == request.UserId && bottle.BottleState == BottleStateEnum.UserDeleteChat)
                    || (bottle.OriginId == request.UserId && bottle.BottleState == BottleStateEnum.OrigionDeleteChat);
                if (bottle != null && !IsUser)
                {
                    //将瓶子初始内容添加进去
                    var dto = new MyBottleDTO()
                    {
                        BottleChatId =0,
                        BottleId = bottle.Id,
                        UserId = bottle.UserId,
                        BottleState = bottle.BottleState,
                        RecentContent = bottle.BottleContent,
                        LastTime = bottle.CreateTime
                    };
                    var bottleUser = loginRepository.GetUserInfoById(bottle.UserId);
                    if (bottleUser != null)
                    {
                        dto.Gender = bottleUser.Gender;
                        dto.NickName = bottleUser.NickName;
                        dto.HeadshotPath = bottleUser.HeadshotPath;
                        //城市信息
                        var city = bottleUser.HomeCityId.HasValue ? commonRepository.GetCityById(bottleUser.HomeCityId.Value) : null;
                        dto.CityName = city == null ? "远方" : city.CityName;
                        //省份信息
                        var province = bottleUser.HomeProvinceId.HasValue ? commonRepository.GetProvinceById(bottleUser.HomeProvinceId.Value) : null;
                        if (cityName.Contains(dto.CityName) &&province != null)
                        {
                            dto.CityName = province.ProvinceName;
                        }
                    }
                    list.Add(dto);
                }
                var chat = discoveryRepository.GetBottleChatById(request.Id.Value);
                foreach (var temp in chat)
                {
                    var bot = discoveryRepository.GetBottlebyId(temp.BottleId);
                    if ((temp.ChatState == BottleChatStateEnum.UserDelete && bot.UserId == request.UserId.Value)//当前登录者是瓶子主人，且主人删了该消息
                       ||(temp.ChatState == BottleChatStateEnum.OrigionDelete && bot.OriginId.Value == request.UserId.Value))//当前登录人时被捡者，且被捡着删了该消息
                    {
                        continue;
                    }
                    var tab = new MyBottleDTO()
                    {
                        BottleChatId=temp.Id,
                        BottleId = temp.BottleId,
                        UserId = temp.UserId.HasValue&&temp.UserId.Value >0? temp.UserId.Value : temp.OriginId.Value,
                        RecentContent = temp.BottleChatContent,
                        LastTime = temp.CreateTime
                    };
                    var bottleUser = loginRepository.GetUserInfoById(tab.UserId);
                    if (bottleUser != null)
                    {
                        tab.Gender = bottleUser.Gender;
                        tab.NickName = bottleUser.NickName;
                        tab.HeadshotPath = bottleUser.HeadshotPath;
                        //城市信息
                        var city = bottleUser.HomeCityId.HasValue ? commonRepository.GetCityById(bottleUser.HomeCityId.Value) : null;
                        tab.CityName = city == null ? "远方" : city.CityName;
                        //省份信息
                        var province = bottleUser.HomeProvinceId.HasValue ? commonRepository.GetProvinceById(bottleUser.HomeProvinceId.Value) : null;
                        if (cityName.Contains(tab.CityName) && province != null)
                        {
                            tab.CityName = province.ProvinceName;
                        }
                    }
                    list.Add(tab);
                }
                rtn.Content = list.OrderByDescending(a => a.LastTime).ToList();
            }
            catch(Exception ex)
            {
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("DiscoveryService", "ReplyBottleList", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 回复瓶子内容
        /// </summary>
        public OperationResult<bool> ReplyBottle(CommonRequest request)
        {
            var rtn = new OperationResult<bool>();
            try
            {
                var bottle = discoveryRepository.GetBottlebyId(request.Id.Value);
                var chat = new BottleChat()
                {
                    BottleId = request.Id.Value,
                    UserId = bottle.UserId == request.UserId ? request.UserId.Value : 0,
                    OriginId = bottle.OriginId == request.UserId ? request.UserId.Value : 0,
                    BottleChatContent = request.Content.Trim(),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                rtn.Content = discoveryRepository.InsertBottleChat(chat);
            }
            catch(Exception ex)
            {
                rtn.Content = false;
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("DiscoveryService", "ReplyBottle", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 删除或者举报瓶子(一级列表）
        /// </summary>
        public OperationResult<bool> UpdateBottle(CommonRequest request)
        {
            var rtn = new OperationResult<bool>();
            try
            {
                var bottle = discoveryRepository.GetBottlebyId(request.Id.Value);
                if(bottle!=null)
                {
                    BottleStateEnum state = BottleStateEnum.Default;
                    if (Convert.ToInt16(request.Content)==1)
                    {
                        state = bottle.UserId == request.UserId.Value ? BottleStateEnum.UserDelete : BottleStateEnum.OrigionDelete;
                    }
                    else
                    {
                        state = BottleStateEnum.Report;
                    }
                    rtn.Content = discoveryRepository.DeleteBottle(state, request.Id.Value);
                }
            }
            catch(Exception ex)
            {
                rtn.Content = false;
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("DiscoveryService", "DeleteBottle", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 删除我的瓶子（二级列表）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperationResult<bool> DeleteBottleReply(CommonRequest request)
        {
            var rtn = new OperationResult<bool>();
            try
            {
                var bottle = discoveryRepository.GetBottlebyId(request.Id.Value);
                var state = bottle.UserId == request.UserId.Value ? BottleStateEnum.UserDelete : BottleStateEnum.OrigionDelete;
                rtn.Content = discoveryRepository.DeleteBottle(state, request.Id.Value);
            }
            catch(Exception ex)
            {
                rtn.Content = false;
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("DiscoveryService", "DeleteBottleReply", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 删除漂流瓶对话（三级列表）
        /// </summary>
        public OperationResult<bool> DeleteBottleChat(CommonRequest request)
        {
            var rtn = new OperationResult<bool>();
            try
            {
                //删除瓶子对话
                if(request.Id !=0) //即BottleChatId!=0,则该消息是ChatContent
                {
                    var bottlechat = discoveryRepository.GetBottleChatById(request.BId.Value).FirstOrDefault();
                    var bottle = discoveryRepository.GetBottlebyId(bottlechat.BottleId);
                    if (bottle != null)
                    {
                        var state = bottle.UserId == request.UserId.Value ? BottleChatStateEnum.UserDelete : BottleChatStateEnum.OrigionDelete;
                        rtn.Content = discoveryRepository.DeleteBottleChat(state, request.Id.Value);
                    }
                }
                //删除瓶子
                else //BottleChatId == 0,则该消息是初始状态加进去的
                {
                    var bottle = discoveryRepository.GetBottlebyId(request.BId.Value);
                    var state = bottle.UserId == request.UserId.Value ? BottleStateEnum.UserDeleteChat : BottleStateEnum.OrigionDeleteChat;
                    rtn.Content = discoveryRepository.DeleteBottle(state, request.BId.Value);
                }
            }
            catch (Exception ex)
            {
                rtn.Content = false;
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("DiscoveryService", "DeleteBottleChat", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 某一漂流瓶详情
        /// </summary>
        public Bottle GetBottle(long bottleId)
        {
            return discoveryRepository.GetBottlebyId(bottleId);
        }
    }
}
