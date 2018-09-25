using Chat.Ultilities.Loger;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Chat.Helper
{
    /// <summary>
    /// 通用类型扩展方法类
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回该类型默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        public static string ToJson(this object value, bool bBigCamelCase = true)
        {
            string json = "";
            if (bBigCamelCase)
            {
                json = JsonConvert.SerializeObject(value);
            }
            else
            {
                json = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            }
            return json;
        }

        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回该类型默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        public static T JsonToObject<T>(this object value)
        {
            T rtn = default(T);
            try
            {
                rtn = JsonConvert.DeserializeObject<T>(value.ToString());
            }
            catch (Exception ex)
            {
                string txt = ("Json值:" + value.ToString() + "转换失败");
                Log.Exception("ObjectExtensions", "JsonToObject", ex, txt);
            }
            return rtn;
        }

        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回该类型默认值,有循环引用时可调用这个函数
        /// </summary>
        /// <param name="value">要转换为Json的对象</param>
        /// <param name="bBigCamelCase">首字母是否大写</param>
        /// <returns>json</returns>
        public static string ToJsonIgnoreLoop(this object value, bool bBigCamelCase = true)
        {
            string json = "";
            try
            {
                if (bBigCamelCase)
                {
                    json = JsonConvert.SerializeObject(value, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
                else
                {
                    json = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                }
            }
            catch(Exception ex)
            {
                string txt = ("Json值:" + value.ToString() + "转换失败");
                Log.Exception("ObjectExtensions", "JsonToObject", ex, txt);
            }
            return json;
        }
    }
}