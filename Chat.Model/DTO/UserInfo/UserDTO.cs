using System;
using System.Configuration;
using Chat.Model.Common.Enum;
using Chat.Ultilities.Extensions;
using Chat.Model.Common;

namespace Chat.Model.DTO.UserInfo
{
    public class UserDTO: User
    {
       
        /// <summary>
        /// 是否记住当前用户
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// 备注名
        /// </summary>
        public string DescName { get; set; }

        /// <summary>
        /// 添加方式
        /// </summary>
        public AddTypeEnum AddType { get; set; }

        /// <summary>
        /// 所在地省
        /// </summary>
        public string HomeProvinceName { get; set; }

        /// <summary>
        /// 所在地城市名
        /// </summary>
        public string HomeCityName { get; set; }

        /// <summary>
        /// 所在地区域
        /// </summary>
        public string HomeAreaName { get; set; }

        /// <summary>
        /// 故乡所在地省
        /// </summary>
        public string HomeTownProvinceName { get; set; }

        /// <summary>
        /// 故乡所在地城市名
        /// </summary>
        public string HomeTownCityName { get; set; }

        /// <summary>
        /// 故乡所在地区域
        /// </summary>
        public string HomeTownAreaName { get; set; }

        /// <summary>
        /// 头像地址（60像素）
        /// </summary>
        public string HeadshotPathDesc
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_60"): ConfigHelper.GetPath("DefaultPath_60", HeadshotPath);
            }
        }

        /// <summary>
        /// 头像地址(50像素）
        /// </summary>
        public string DefaultPath_50
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_50") : ConfigHelper.GetPath("DefaultPath_50", HeadshotPath);
            }
        }
        /// <summary>
        /// 添加方式转义
        /// </summary>
        public string AddTypeDesc
        {
            get
            {
                return AddType.ToDescription();
            }
        }

    }
}
