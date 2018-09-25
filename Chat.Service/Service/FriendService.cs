using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Repository;
using Chat.Service.IService;
using Chat.Model.DTO.Friend;
using System.Collections.Generic;
using Chat.Model.Common.Enum;

namespace Chat.Service
{
    public class FriendService: IFriendService
    {
        private FriendRepository friendRepository = new FriendRepository();

        /// <summary>
        /// 添加好友
        /// </summary>
        public OperationResult<bool> AddValidate(AddRequestDTO req)
        {
            var op = new OperationResult<bool>();
            var add= friendRepository.AddValidate(req);
            if(add&& req.AddStat== AddStatEnum.Pass)
            {
                var addreq = friendRepository.AddRequest(req.Id);
                //数据库插入2条数据
                var adduser = friendRepository.AddFriend(addreq.ReqUserId, addreq.ReqOriginId, addreq.AddType, addreq.DescName);
                var addorigion = false;
                if (adduser)
                {
                    //给对方的备注初始为空
                    addorigion = friendRepository.AddFriend(addreq.ReqOriginId, addreq.ReqUserId, addreq.AddType);
                }
                op.Content = addorigion;
            }
            return op;
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        public BootstrapTableResponse<FriendDTO> GetFriendList(long userId)
        {
            return friendRepository.GetFriendList(userId);
        }

        /// <summary>
        /// 获取新好友列表
        public OperationResult<List<AddRequestDTO>> GetNewFriendList(long userId)
        {
            return friendRepository.GetNewFriendList(userId);
        }
    }
}
