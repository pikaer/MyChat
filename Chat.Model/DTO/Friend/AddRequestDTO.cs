using Chat.Model.Common;
using Chat.Model.Common.Enum;
using Chat.Model.Entity.FriendInfo;
using Chat.Ultilities.Extensions;
using System;
using System.Configuration;

namespace Chat.Model.DTO.Friend
{
    public class AddRequestDTO : AddRequest
    {

        #region get;set;
        /// <summary>
        /// 被动加好友（对方加自己为好友）
        /// </summary>
        public bool IsPassive { get; set; }

        /// <summary>
        /// 好友性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadshotPath { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        #endregion

        #region get;
        /// <summary>
        /// 好友添加状态转义
        /// </summary>
        public string AddStatDesc
        {
            get
            {
                string desc = "";
                //对方添加自己，直接转换为描述
                if(IsPassive)
                {
                    desc=AddStat.ToDescription();
                }
                else
                {
                    if(AddStat== AddStatEnum.Pass)
                    {
                        desc = AddStat.ToDescription();
                    }
                    else
                    {
                        desc = "等待验证";
                    }
                }
                return desc;
            }
        }

        /// <summary>
        /// 好友添加方式转义
        /// </summary>
        public string AddTypeDesc
        {
            get
            {
                return AddType.ToDescription();
            }
        }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadshotPathDesc
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_50") : ConfigHelper.GetPath("DefaultPath_50", HeadshotPath);
            }
        }

        /// <summary>
        /// 好友年龄
        /// </summary>
        public string Age
        {
            get
            {
                return BirthDay.HasValue?DateTimeExtensions.GetAgeByBirthdate(BirthDay.Value).ToString():"";
            }
        }

        /// <summary>
        ///好友性别
        /// </summary>
        public string GenderDesc
        {
            get
            {
                return Gender.ToDescription();
            }
        }

        public string DescNameStr
        {
            get
            {
                return string.IsNullOrEmpty(DescName)? NickName:DescName;
            }
        }
        #endregion
    }
}
