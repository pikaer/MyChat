using Chat.Model.Common.Enum;

namespace Chat.Model.Common.Request
{
    public class AddFriendRequest
    {
        /// <summary>
        /// 查找关键词
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 年龄段
        /// </summary>
        public AgeEnum Age { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool onLine { get; set; }

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
        public int? HomeTownAreaId{get;set;}

        /// <summary>
        /// 故乡所在城市
        /// </summary>
        public int? HomeTownCityId { get; set; }

        /// <summary>
        /// 故乡所在省份
        /// </summary>
        public int? HomeTownProvinceId { get; set; }
    }
}
