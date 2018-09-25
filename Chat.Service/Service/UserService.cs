using Chat.Model.Common.Helper;
using Chat.Model;
using Chat.Repository;
using Chat.Service.IService;
using System.Collections.Generic;
using Chat.Model.DTO.UserInfo;
using System;
using Chat.Model.Common;
using Chat.Model.Common.Request;
using Chat.Model.Common.Response;
using System.Configuration;
using Chat.Ultilities.Extensions;
using System.IO;
using Chat.Ultilities.Loger;

namespace Chat.Service.Service
{
    public class UserService : IUserService
    {
        private LoginRepository loginRepository = new LoginRepository();
        private UserRepository userRepository = new UserRepository();
        private CommonRepository commonRepository = new CommonRepository();
        private CommonService commonService = new CommonService();
        private string upLoadPath100 = ConfigurationManager.AppSettings["UploadPath_100"];
        private string upLoadPath60 = ConfigurationManager.AppSettings["UploadPath_60"];
        private string upLoadPath50 = ConfigurationManager.AppSettings["UploadPath_50"];
        public OperationResult<List<User>> GetUserList()
        {
            return userRepository.GetUserList();
        }

        /// <summary>
        /// 好友详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperationResult<UserDTO> UserDetail(CommonRequest request)
        {
            var rtn = new OperationResult<UserDTO>();
            //好友基础信息
            var user = loginRepository.GetUserInfoById(request.OriginId.Value);
            //好友备注信息
            var friend = userRepository.UserDetail(request);
            if (user != null)
            {
                //省份信息
                var province = user.HomeProvinceId.HasValue ? commonRepository.GetProvinceById(user.HomeProvinceId.Value) : null;
                //城市信息
                var city = user.HomeCityId.HasValue ? commonRepository.GetCityById(user.HomeCityId.Value) : null;
                //区域信息
                var area = user.HomeAreaId.HasValue ? commonRepository.GetAreaById(user.HomeAreaId.Value) : null;

                user.HomeProvinceName = province == null ? "" : province.ProvinceName;
                user.HomeCityName = city == null ? "" : city.CityName;
                user.HomeAreaName = area == null ? "" : area.AreaName;
                user.DescName = string.IsNullOrEmpty(friend.DescName) ? user.NickName : friend.DescName;
                user.AddType = friend.AddType;
                rtn.Content = user;
            }
            return rtn;
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperationResult<bool> DeleteFriend(CommonRequest request)
        {
            return userRepository.DeleteFriend(request);
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public OperationResult<bool> UpLoadHeadPhoto(UpLoadPhotoRequest req, long userId, string path)
        {
            var rtn = new OperationResult<bool>();
            try
            {
                Guid guid = Guid.NewGuid();
                string pathMake = path + guid.ToString() + ".png";
                string pathOp = path.Replace("UserInfo\\upfiles", "");
                string path100 = pathOp + upLoadPath100 + guid.ToString() + ".png";
                string path60 = pathOp + upLoadPath60 + guid.ToString() + ".png";
                string path50 = pathOp + upLoadPath50 + guid.ToString() + ".png";

                //裁剪图片
                bool imageRepair = Image.ImageRepair(req.Path, pathMake, (int)req.width, (int)req.height, (int)req.x, (int)req.y);
                if (imageRepair)
                {
                    //缩略图100像素
                    bool save100 = Image.Thumbnail(pathMake, path100, 100, 100);
                    if (save100)
                    {
                        //缩略图60像素
                        bool save60 = Image.Thumbnail(pathMake, path60, 60, 60);
                        if (save60)
                        {
                            //缩略图50像素
                            bool save50 = Image.Thumbnail(pathMake, path50, 50, 50);
                            if (save50)
                            {
                                bool save = userRepository.UpdateHeadPath(guid.ToString(), userId);
                                if (save)
                                {
                                    rtn.Content=true;
                                }
                            }
                            else
                            {
                                //删除100像素图片
                                File.Delete(path100);
                                //删除60像素图片
                                File.Delete(path60);
                                rtn.ResultType = OperationResultType.Error;
                            }
                        }
                        else
                        {
                            //删除100像素图片
                            File.Delete(path100);
                            rtn.ResultType = OperationResultType.Error;
                        }
                    }
                }
                //压缩图片完，删除裁剪文件
                File.Delete(pathMake);
            }
            catch (Exception ex)
            {
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("UserService", "UpLoadHeadPhoto", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 获取用户详情页数据
        /// </summary>
        public OperationResult<BasicInfoDTO> GetBasicInfo(long userId)
        {
            var rtn = new OperationResult<BasicInfoDTO>();
            try
            {
                var user = userRepository.GetUserById(userId);
                rtn.Content = user;
                if(user.HomeProvinceId.HasValue&& user.HomeCityId.HasValue&& user.HomeAreaId.HasValue)
                {
                    rtn.Content.HomeDesc = commonService.HomeDesc(user.HomeProvinceId.Value, user.HomeCityId.Value, user.HomeAreaId.Value);
                }
                if(user.HomeTownProvinceId.HasValue && user.HomeTownCityId.HasValue && user.HomeTownAreaId.HasValue)
                {
                    rtn.Content.HomeTownDesc = commonService.HomeTownDesc(user.HomeTownProvinceId.Value, user.HomeTownCityId.Value, user.HomeTownAreaId.Value);
                }
            }
            catch (Exception ex)
            {
                rtn.ResultType = OperationResultType.Error;
                Log.Exception("UserService", "GetBasicInfo", ex);
            }
            return rtn;
        }

        /// <summary>
        /// 编辑用户详情
        /// </summary>
        public OperationResult<bool> BasicInfoEdit(BasicInfoDTO request)
        {
            var rtn = new OperationResult<bool>();
            request.UpdateTime = DateTime.Now;
            rtn.Content = userRepository.BasicInfoEdit(request);
            return rtn;
        }
        
    }
}
