using Chat.Model;
using Chat.Model.Common;
using Chat.Model.Common.Enum;
using Chat.Service.IService;
using Chat.Service.Service;
using Chat.Ultilities.Extensions;
using System.Web.Mvc;
using Chat.Helper;

namespace Chat.Areas.WebSite.Controllers
{
    /// <summary>
    /// 公共控制器
    /// </summary>
    public class CommonController : BaseController
    {
        private ICommonService commonService = new CommonService();
        /// <summary>
        /// 性别下拉列表
        /// </summary>
        public JsonResult GenderCombobox()
        {
            var list = EnumExtensions.ToSelectPicker(typeof(GenderEnum));
            return Json(new JsonMess(true, "", list));
        }

        public JsonResult GenderComboboxV1()
        {
            var list = EnumExtensions.ToSelectPicker(typeof(GenderEnum));
            list[0].SelectValue = "";
            return Json(new JsonMess(true, "", list));
        }

        /// <summary>
        /// 年龄范围下拉列表
        /// </summary>
        public JsonResult AgeCombobox()
        {
            var list = EnumExtensions.ToSelectPicker(typeof(AgeEnum));
            return Json(new JsonMess(true, "", list));
        }

        /// <summary>
        /// 省份下拉列表框
        /// </summary>
        public JsonResult ProvinceCombobox()
        {
            var response = commonService.ProvinceCombobox();
            return Json(response.JsonMess());
        }
        /// <summary>
        /// 市下拉列表
        /// </summary>
        public JsonResult CityCombobox(string data)
        {
            var common = data.JsonToObject<CommonRequest>();
            int provinceId = common.ProvinceId.HasValue ? common.ProvinceId.Value : 0;
            var response = commonService.CityCombobox(provinceId);
            return Json(response.JsonMess());
        }
        /// <summary>
        /// 区域下拉列表
        /// </summary>
        public JsonResult AreaCombobox(string data)
        {
            var common= data.JsonToObject<CommonRequest>();
            int cityId = common.CityId.HasValue ? common.CityId.Value : 0;
            var response = commonService.AreaCombobox(cityId);
            return Json(response.JsonMess());
        }

        /// <summary>
        /// 血型下拉列表
        /// </summary>
        public JsonResult BloodTypeCombobox(string data)
        {
            var list = EnumExtensions.ToSelectPicker(typeof(BloodTypeEnum));
            return Json(new JsonMess(true, "", list));
        }

        /// <summary>
        /// 年份下拉列表
        /// </summary>
        public JsonResult YearCombobox()
        {
            return Json(new JsonMess(true, "", DateTimeExtensions.YearSelect()));
        }

        /// <summary>
        /// 月份下拉列表
        /// </summary>
        public JsonResult MonthCombobox()
        {
            return Json(new JsonMess(true, "", DateTimeExtensions.MonthSelect()));
        }

        /// <summary>
        /// 日期下拉列表
        /// </summary>
        public JsonResult DayCombobox(string data)
        {
            var common = data.JsonToObject<CommonRequest>();
            return Json(new JsonMess(true, "", DateTimeExtensions.DaySelect((int)common.Id,(int)common.BId)));
        }
    }
}