using Chat.Model.Common;
using Chat.Model.Common.Helper;
using Chat.Ultilities.Common;
using Chat.Ultilities.Loger;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Chat.Model.Entity.Provinces;

namespace Chat.Repository
{
    public class CommonRepository:BaseRepository
    {
        protected override BaseRepository.DbEnum GetDbEnum()
        {
            return DbEnum.MyChat;
        }

        /// <summary>
        /// 省信息键值对
        /// </summary>
        /// <returns></returns>
        public OperationResult<List<SelectPicker>> ProvinceCombobox()
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<List<SelectPicker>>();
                try
                {
                    var sql = @"SELECT ProvinceId as  SelectKey
                                  ,ProvinceName as SelectValue
                            FROM dbo.provinces_Province";
                    result.Content = Db.Query<SelectPicker>(sql).ToList();
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("CommonRepository", "ProvinceCombobox", ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 市信息键值对
        /// </summary>
        public OperationResult<List<SelectPicker>> CityCombobox(int provinceId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<List<SelectPicker>>();
                try
                {
                    var sql = @"SELECT CityId as  SelectKey
                                  ,CityName as SelectValue
                            FROM dbo.provinces_City 
                            Where ProvinceId=@ProvinceId";
                    result.Content = Db.Query<SelectPicker>(sql, new { ProvinceId = provinceId }).ToList();
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("CommonRepository", "CityCombobox", ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 区域信息键值对
        /// </summary>
        public OperationResult<List<SelectPicker>> AreaCombobox(int cityId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new OperationResult<List<SelectPicker>>();
                try
                {
                    var sql = @"SELECT AreaId as  SelectKey
                                  ,AreaName as SelectValue
                            FROM dbo.provinces_Area 
                            Where CityId=@CityId";
                    result.Content = Db.Query<SelectPicker>(sql, new { CityId = cityId }).ToList();
                }
                catch (Exception ex)
                {
                    result.ResultType = OperationResultType.Error;
                    result.Message = "数据库操作失败！";
                    Log.Exception("CommonRepository", "AreaCombobox", ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 通过provinceId获取省信息
        /// </summary>
        public Province GetProvinceById(int provinceId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new Province();
                try
                {
                    var sql = @"SELECT ProvinceId
                                      ,ProvinceName
                                  FROM dbo.provinces_Province
                                  WHERE ProvinceId=@Id";
                    result= Db.QueryFirstOrDefault<Province>(sql, new { Id = provinceId });
                }
                catch (Exception ex)
                {
                    Log.Exception("CommonRepository", "GetProvinceById", ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 通过Id获取城市信息
        /// </summary>
        public City GetCityById(int cityId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new City();
                try
                {
                    var sql = @"SELECT CityId
                                      ,ProvinceId
                                      ,CityName
                                  FROM dbo.provinces_City
                                  WHERE CityId=@Id";
                    result = Db.QueryFirstOrDefault<City>(sql, new { Id = cityId });
                }
                catch (Exception ex)
                {
                    Log.Exception("CommonRepository", "GetCityById", ex);
                }
                return result;
            }
        }

        /// <summary>
        /// 根据Id获取区域信息
        /// </summary>
        public Area GetAreaById(int areaId)
        {
            using (var Db = GetDbConnection())
            {
                var result = new Area();
                try
                {
                    var sql = @"SELECT AreaId
                                      ,CityId
                                      ,AreaName
                                  FROM dbo.provinces_Area
                                  WHERE AreaId=@Id";
                    result = Db.QueryFirstOrDefault<Area>(sql, new { Id = areaId });
                }
                catch (Exception ex)
                {
                    Log.Exception("CommonRepository", "GetAreaById", ex);
                }
                return result;
            }
        }
    }
}
