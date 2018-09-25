using Chat.Model.Common.Enum;
using System;

namespace Chat.Model
{
    /// <summary>
    /// 已注册用户类
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string HeadshotPath { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public int? HomeAreaId { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        public int? HomeCityId { get; set; }

        /// <summary>
        /// 所在省份
        /// </summary>
        public int? HomeProvinceId { get; set; }

        /// <summary>
        /// 故乡所在区域
        /// </summary>
        public int? HomeTownAreaId { get; set; }

        /// <summary>
        /// 故乡所在城市
        /// </summary>
        public int? HomeTownCityId { get; set; }

        /// <summary>
        /// 故乡所在省份
        /// </summary>
        public int? HomeTownProvinceId { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 血型
        /// </summary>
        public BloodTypeEnum BloodType { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public long QQ { get; set; }
    }
}
