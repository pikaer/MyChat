using Chat.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Chat.Helper;
using Chat.Model.DTO.Chat;
using Chat.Service.IService;
using Chat.Service.Service;

namespace Chat
{
    /// <summary>
    /// 聊天页面集线器
    /// 一对一聊天
    /// </summary>
    public class ChatHub : BaseHub
    {
        //当前在线用户集合
        private static List<HubDTO> _userList = new List<HubDTO>();
        private IChatContentService chatContentService = new ChatContentService();

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
            var user = _userList.Where(a => a.ConnectionId == Context.ConnectionId||a.UserId==CurrentUser.Id).FirstOrDefault();
            if(user!=null)
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

        /// <summary>
        /// 发送消息给特定用户
        /// </summary>
        public void SendMessageToUser(string data)
        {
            var request = data.JsonToObject<ChatHistoryDTO>();
            //存到数据库
            var response = chatContentService.AddPersonalChatHistory(request, CurrentUser.Id);

            //如果对方在线，则通知对方获取此消息
            if (_userList.Exists(a=>a.UserId== request.OriginId))
            {
                var user = _userList.Where(a => a.UserId == request.OriginId).FirstOrDefault();
                //服务器端通知，客户端自己获取数据
                Clients.Client(user.ConnectionId).PushMessgeToUser(CurrentUser.Id.ToString());
            }
        }
        #endregion
    }
}