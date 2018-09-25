using Chat.Service.IService;
using Chat.Model;
using Chat.Model.DTO.Add;
using Chat.Model.DTO.UserInfo;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Chat.Model.Common.Helper;
using Chat.Model.Entity.FriendInfo;
using Chat.Repository;
using System;
using Chat.Model.Common.Enum;
using Chat.Model.Common.Request;
using Chat.Model.Common;
using Chat.Ultilities.Loger;

namespace Chat.Service.Service
{
    public class AddService : IAddService
    {
        private AddRepository addRepository = new AddRepository();
        private UserRepository userRepository = new UserRepository();
        private string headshotPath = ConfigurationManager.AppSettings["DefaultHeadshotPath_60"];
        /// <summary>
        /// 查找页面用户列表
        /// </summary>
        /// <returns></returns>
        public BootstrapTableResponse<UserTableDTO> GetUserList(AddFriendRequest req, long Id)
        {
            var rtn = new BootstrapTableResponse<UserTableDTO>();
            var list = userRepository.UserListByParam(req,Id);
            
            //把List均分到四个子集中
            int row = list.Count / 4;   //取整
            var remain= list.Count / 4; //取余
            row = remain == 0 ? row : row + 1;
            var dic = new Dictionary<int, List<UserDTO>>();
            for (int i=0;i<row; i++)
            {
                var child = list.Skip(i*4).Take(4).ToList();
                dic.Add(i, child);
            }
            for(int i=0;i<row;i++)//行
            {
                var tab = new UserTableDTO();
                for (int j=0;j<4;j++) //列
                {
                    if(j>dic[i].Count-1)
                    {
                        continue;
                    }
                    else
                    {
                        var dto = dic[i][j];
                        if (j == 0)
                        {
                            tab.Column1 = dto;
                        }
                        else if (j == 1)
                        {
                            tab.Column2 = dto;
                        }
                        else if (j == 2)
                        {
                            tab.Column3 = dto;
                        }
                        else
                        {
                            tab.Column4 = dto;
                        }
                    }
                }
                rtn.rows.Add(tab);
            }
            rtn.total = row;
            return rtn;
        }

        /// <summary>
        /// 添加好友请求
        /// </summary>
        public OperationResult<bool> AddFriend(AddRequest request)
        {
            request.CreateTime = DateTime.Now;
            request.UpdateTime = DateTime.Now;
            request.AddStat = AddStatEnum.Wait;
            request.DescName = string.IsNullOrEmpty(request.DescName) ? null : request.DescName;
            return addRepository.AddFriend(request);
        }

        //删除添加好友请求数据
        public OperationResult<bool> DeleteAddRequest(CommonRequest request)
        {
            var rtn = new OperationResult<bool>()
            {
                Content = false,
                ResultType = OperationResultType.Error
            };
            try
            {
                var dto = addRepository.GetAddDetail(request.Id.Value);
                if(dto!=null)
                {
                    if(dto.ReqUserId == request.UserId.Value)
                    {
                        dto.IsUserDelete = true;
                    }
                    if (dto.ReqOriginId == request.UserId.Value)
                    {
                        dto.IsOriginDelete = true;
                    }
                    rtn.Content = addRepository.DeleteAddRequest(dto);
                    rtn.ResultType = OperationResultType.Success;
                }
            }
            catch(Exception ex)
            {
                Log.Exception("FriendRepository", "GetNewFriendList", ex);
            }
            return rtn;
        }
    }
}
