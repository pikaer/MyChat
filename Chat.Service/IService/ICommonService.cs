using Chat.Model.Common.Helper;
using Chat.Ultilities.Common;
using System.Collections.Generic;

namespace Chat.Service.IService
{
    public interface ICommonService
    {
        /// <summary>
        /// 省键值对
        /// </summary>
        OperationResult<List<SelectPicker>> ProvinceCombobox();

        /// <summary>
        /// 城市键值对
        /// </summary>
        OperationResult<List<SelectPicker>> CityCombobox(int provinceId);

        /// <summary>
        /// 区域键值对
        /// </summary>
        OperationResult<List<SelectPicker>> AreaCombobox(int cityId);
    }
}
