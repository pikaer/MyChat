using Chat.Ultilities.Common;
using Chat.Ultilities.Loger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Ultilities.Extensions
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        public static Dictionary<int, string> ToDictionary(Type enumType)
        {
            Dictionary<int, string> listitem = new Dictionary<int, string>();
            Array vals = Enum.GetValues(enumType);
            foreach (Enum enu in vals)
            {
                listitem.Add(Convert.ToInt32(enu), enu.ToDescription());
            }
            return listitem;
        }

        public static List<SelectPicker> ToSelectPicker(Type enumType)
        {
            var listitem =new List<SelectPicker>();
            Array vals = Enum.GetValues(enumType);
            foreach (Enum enu in vals)
            {
                var selectPicker = new SelectPicker()
                {
                    SelectKey = Convert.ToInt32(enu),
                    SelectValue = enu.ToDescription()
                };
                listitem.Add(selectPicker);
            }
            return listitem;
        }
        /// <summary>
        /// 获取枚举项的Description特性的描述文字
        /// </summary>
        /// <param name="enumeration"> </param>
        /// <returns> </returns>
        public static string ToDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] members = type.GetMember(enumeration.CastTo<string>());
            if (members.Length > 0)
            {
                return members[0].ToDescription();
            }
            return "";
        }

        /// <summary>
        /// 可能有多重描述
        /// 适用 值形式 1 2 4 8 16....
        /// </summary>
        /// <param name="enumeration"></param>
        /// <param name="enum1"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string ToMultiDescription(this Enum enumeration, char split = ' ')
        {
            Type type = enumeration.GetType();
            string objName = enumeration.CastTo<string>();
            string result = "";
            MemberInfo[] members = type.GetMember(objName);
            if (members.Length > 0)
            {
                result = members[0].ToDescription();
            }
            else
            {
                string[] strs = objName.Split(',');//CastTo结果是由逗号分隔的
                if (strs.Length == 0)
                {
                    result = objName;
                }
                else
                {
                    for (int i = 0; i < strs.Length; i++)
                    {
                        MemberInfo[] tmpMembers = type.GetMember(strs[i].Trim());
                        if (tmpMembers.Length > 0)
                        {
                            result += tmpMembers[0].ToDescription() + split;
                        }
                    }
                }
            }

            return result.Trim(split);
        }

        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回该类型默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        public static T CastTo<T>(this object value)
        {
            object result;
            Type type = typeof(T);
            try
            {
                if (type.IsEnum)
                {
                    result = Enum.Parse(type, value.ToString());
                }
                else if (type == typeof(Guid))
                {
                    result = Guid.Parse(value.ToString());
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }
            catch (Exception ex)
            {
                result = default(T);
                Log.Exception("ObjectExtensions", "CastTo", ex,"入参为："+value);
            }
            return (T)result;
        }
    }
}
