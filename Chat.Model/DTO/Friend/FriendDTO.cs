using Chat.Model.Common;
using System;

namespace Chat.Model.DTO.Friend
{
    public class FriendDTO
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 好友备注名
        /// </summary>
        public string DescName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadshotPath { get; set; }


        public string DescNameStr
        {
            get
            {
                var txt = DescName.Replace(" ","");
                return string.IsNullOrEmpty(txt) ? NickName : DescName;
            }
        }
        public string HeadshotPathDesc
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_50") : ConfigHelper.GetPath("DefaultPath_50", HeadshotPath);
            }
        }
    }
}
