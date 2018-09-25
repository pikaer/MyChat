using Chat.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Chat.Helper;
using Chat.Service.IService;
using Chat.Service.Service;
using Chat.Model.Common.Enum;
using Chat.Model.Common;

namespace Chat.ServerHub
{
    /// <summary>
    /// 漂流瓶集线器
    /// </summary>
    public class BottleHub : BaseHub
    {
        //当前在线用户集合
        private static List<HubDTO> _userList = new List<HubDTO>();
        private IBottleService discoveryService = new BottleService();
       
        #region 重写虚方法
        /// <summary>
        /// 用户连接的时候
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            if (_userList.Count(a => a.UserId == CurrentUser.Id) != 0)
            {
                //当重新连接时，先移除上次记录的连接参数
                var user = _userList.Where(a => a.UserId == CurrentUser.Id).FirstOrDefault();
                _userList.Remove(user);
            }
            //重置在线用户
            _userList.Add(new HubDTO()
            {
                ConnectionId = Context.ConnectionId,
                UserId = CurrentUser.Id,
                ConnectTime = DateTime.Now
            });
            return base.OnConnected();
        }

        /// <summary>
        /// 断线时
        /// </summary>
        /// <param name="stopCalled">是否停止通信</param>
        public override Task OnDisconnected(bool stopCalled)
        {
            var user = _userList.Where(a => a.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (user != null)
            {
                _userList.Remove(user);
            }
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            if (_userList.Count(a => a.UserId == CurrentUser.Id) != 0)
            {
                //当重新连接时，先移除上次记录的连接参数
                var user = _userList.Where(a => a.UserId == CurrentUser.Id).FirstOrDefault();
                _userList.Remove(user);
            }
            //重置在线用户
            _userList.Add(new HubDTO()
            {
                ConnectionId = Context.ConnectionId,
                UserId = CurrentUser.Id,
                ConnectTime = DateTime.Now
            });
            return base.OnReconnected();
        }

        #endregion

        #region 前后端通信
        public void ReplyBottle(string data)
        {
            var request = data.JsonToObject<CommonRequest>();
            request.UserId = CurrentUser.Id;
            //获取瓶子
            var bottle = discoveryService.GetBottle(request.Id.Value);
            if(bottle.BottleState == BottleStateEnum.Default|| bottle.BottleState == BottleStateEnum.Received)
            {
                //插入数据库
                var rtn = discoveryService.ReplyBottle(request);
                if(rtn.ResultType== OperationResultType.Success)
                {
                    //瓶子对方
                    long origionId = bottle.UserId == CurrentUser.Id ? bottle.OriginId.Value : bottle.UserId;
                    //如果对方在线，则推送至前端
                    if (_userList.Exists(a => a.UserId == origionId))
                    {
                        var user = _userList.Where(a => a.UserId == origionId).FirstOrDefault();
                        //将瓶子Id传递到前端，通知对方刷新
                        Clients.Client(user.ConnectionId).PushReplyToUser(bottle.Id.ToString());
                    }
                }
            }
        }
        #endregion
    }
}