using Chat.Model.Common;
using Chat.Model.Common.Enum;
using Chat.Ultilities.Extensions;

namespace Chat.Model.DTO.UserInfo
{
    /// <summary>
    /// 用户详情
    /// </summary>
    public class BasicInfoDTO:User
    {
        public bool IsHomeChange { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string BirthDayDesc
        {
            get
            {
                return BirthDay.HasValue?BirthDay.Value.ToString("D"):"";
            }
        }

        /// <summary>
        /// 家乡
        /// </summary>
        public string HomeTownDesc { get; set; }

        /// <summary>
        /// 所在地
        /// </summary>
        public string HomeDesc { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string GenderDesc
        {
            get
            {
                return Gender == GenderEnum.Default ? "":Gender.ToDescription();
            }
        }

        /// <summary>
        /// 血型
        /// </summary>
        public string BloodTypeDesc
        {
            get
            {
                return BloodType.ToDescription();
            }
        }

        /// <summary>
        /// 头像地址(100像素）
        /// </summary>
        public string HeadshotPathDesc
        {
            get
            {
                return string.IsNullOrEmpty(HeadshotPath) ? ConfigHelper.GetConfig("DefaultHeadshotPath_100") : ConfigHelper.GetPath("DefaultPath_100", HeadshotPath);
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

        public string Year
        {
            get
            {
                return BirthDay.HasValue?BirthDay.Value.Year.ToString():"";
            }
        }

        public string Month
        {
            get
            {
                return BirthDay.HasValue ? BirthDay.Value.Month.ToString() : "";
            }
        }
        public string Day
        {
            get
            {
                return BirthDay.HasValue ? BirthDay.Value.Day.ToString() : "";
            }
        }
    }
}
