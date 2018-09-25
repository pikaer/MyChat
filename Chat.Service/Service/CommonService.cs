using Chat.Model;
using Chat.Model.Common.Helper;
using Chat.Repository;
using Chat.Service.IService;
using Chat.Ultilities.Common;
using System.Collections.Generic;

namespace Chat.Service.Service
{
    public class CommonService : ICommonService
    {
        private CommonRepository commonRepository = new CommonRepository();

        /// <summary>
        /// 省键值对
        /// </summary>
        public OperationResult<List<SelectPicker>> ProvinceCombobox()
        {
            return commonRepository.ProvinceCombobox();
        }

        /// <summary>
        /// 城市键值对
        /// </summary>
        public OperationResult<List<SelectPicker>> CityCombobox(int provinceId)
        {
            return commonRepository.CityCombobox(provinceId);
        }

        /// <summary>
        /// 区域键值对
        /// </summary>
        public OperationResult<List<SelectPicker>> AreaCombobox(int cityId)
        {
            return commonRepository.AreaCombobox(cityId);
        }

        /// <summary>
        /// 所在地字符串拼接
        /// </summary>
        public string HomeDesc(int provinceId,int cityId,int areaId)
        {
            //Home
            var homePro = commonRepository.GetProvinceById(provinceId);
            var homeProName = homePro == null ? "" : homePro.ProvinceName.Trim();
            var homeCity = commonRepository.GetCityById(cityId);
            var homeCityName = homeCity == null ? "" : homeCity.CityName.Trim();
            var homeArea = commonRepository.GetAreaById(areaId);
            var homeAreaName = homeArea == null ? "" : homeArea.AreaName.Trim();
            return homeProName + " " + homeCityName + " " + homeAreaName;
        }

        /// <summary>
        /// 家乡所在地字符串拼接
        /// </summary>
        public string HomeTownDesc(int provinceId, int cityId, int areaId)
        {
            //HomeTown
            var homeTownPro = commonRepository.GetProvinceById(provinceId);
            var homeTownProName = homeTownPro == null ? "" : homeTownPro.ProvinceName.Trim();
            var homeTownCity = commonRepository.GetCityById(cityId);
            var homeTownCityName = homeTownCity == null ? "" : homeTownCity.CityName.Trim();
            var homeTownArea = commonRepository.GetAreaById(areaId);
            var homeTownAreaName = homeTownArea == null ? "" : homeTownArea.AreaName.Trim();
            return homeTownProName + " " + homeTownCityName + " " + homeTownAreaName;
        }
    }
}
