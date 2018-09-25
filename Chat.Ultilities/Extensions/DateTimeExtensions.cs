using Chat.Ultilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Ultilities.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 时间转义
        /// </summary>
        /// <param name="datetime">时间：2018-9-4：6：30：30.000</param>
        /// <returns></returns>
        public static string GetDateDesc(DateTime datetime)
        {
            var rtn = "";
            var now = DateTime.Now;
            var today = DateTime.Now.Date;  //2018-9-4 0:00:00 今天凌晨
            var yestoday = DateTime.Now.AddDays(-1).Date;  //2018-9-3 0:00:00  昨天凌晨
            var beforeyestoday = DateTime.Now.AddDays(-2).Date;  //2018-9-2 0:00:00  前天凌晨
            if(datetime>today)
            {
                var min1=DateTime.Now.AddMinutes(-1).Date;       //    1分钟前
                var min2=DateTime.Now.AddMinutes(-2).Date;       //    2分钟前
                var min5=DateTime.Now.AddMinutes(-5).Date;       //    5分钟前
                var min10=DateTime.Now.AddMinutes(-10).Date;     //    10分钟前
                var min20=DateTime.Now.AddMinutes(-20).Date;     //    20分钟前
                var min30=DateTime.Now.AddMinutes(-30).Date;     //    30分钟前
                var hour1 = DateTime.Now.AddHours(-1).Date;      //    1小时前
                var hour2 = DateTime.Now.AddHours(-2).Date;      //    2小时前
                var hour3 = DateTime.Now.AddHours(-3).Date;      //    3小时前
                var hour4 = DateTime.Now.AddHours(-4).Date;      //    4小时前
                var hour5 = DateTime.Now.AddHours(-5).Date;      //    5小时前
                var hour6 = DateTime.Now.AddHours(-6).Date;      //    6小时前
                var hour7 = DateTime.Now.AddHours(-7).Date;      //    7小时前
                var hour8 = DateTime.Now.AddHours(-8).Date;      //    8小时前
                var hour9 = DateTime.Now.AddHours(-9).Date;      //    9小时前
                var hour10 = DateTime.Now.AddHours(-10).Date;    //    10小时前
                var hour11 = DateTime.Now.AddHours(-11).Date;    //    11小时前
                var hour12 = DateTime.Now.AddHours(-12).Date;    //    12小时前
                var hour13 = DateTime.Now.AddHours(-13).Date;    //    13小时前
                var hour14 = DateTime.Now.AddHours(-14).Date;    //    14小时前
                if(datetime>=min1)
                {
                    rtn = "刚刚";
                }
                else if(datetime>=min2&&datetime<min1)
                {
                    rtn = "1分钟前";
                }
                else if (datetime >= min5 && datetime < min2)
                {
                    rtn = "2分钟前";
                }
                else if (datetime >= min10 && datetime < min5)
                {
                    rtn = "5分钟前";
                }
                else if (datetime >= min20 && datetime < min10)
                {
                    rtn = "10分钟前";
                }
                else if (datetime >= min30 && datetime < min20)
                {
                    rtn = "20分钟前";
                }
                else if (datetime >= hour1 && datetime < min30)
                {
                    rtn = "30分钟前";
                }
                else if (datetime >= hour2 && datetime < hour1)
                {
                    rtn = "1小时前";
                }
                else if (datetime >= hour3 && datetime < hour2)
                {
                    rtn = "2小时前";
                }
                else if (datetime >= hour4 && datetime < hour3)
                {
                    rtn = "3小时前";
                }
                else if (datetime >= hour5 && datetime < hour4)
                {
                    rtn = "4小时前";
                }
                else if (datetime >= hour6 && datetime < hour5)
                {
                    rtn = "5小时前";
                }
                else if (datetime >= hour7 && datetime < hour6)
                {
                    rtn = "6小时前";
                }
                else if (datetime >= hour8 && datetime < hour7)
                {
                    rtn = "8小时前";
                }
                else if (datetime >= hour10 && datetime < hour9)
                {
                    rtn = "9小时前";
                }
                else if (datetime >= hour11 && datetime < hour10)
                {
                    rtn = "10小时前";
                }
                else if (datetime >= hour12 && datetime < hour11)
                {
                    rtn = "11小时前";
                }
                else if (datetime >= hour13 && datetime < hour12)
                {
                    rtn = "12小时前";
                }
                else if (datetime >= hour14 && datetime < hour13)
                {
                    rtn = "13小时前";
                }
                else 
                {
                    rtn = "14小时前";
                }
            }
            else if (datetime <= today && datetime > yestoday)
            {
                rtn = "昨天";
            }
            else if (datetime <= yestoday && datetime > beforeyestoday)
            {
                rtn = "前天";
            }
            else
            {
                rtn = datetime.ToShortDateString().ToString();
            }
            return rtn;
        }

        /// <summary>
        /// 计算年龄
        /// </summary>
        /// <param name="birthdate">生日</param>
        /// <returns></returns>
        public static int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        /// <summary>
        /// 年份下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectPicker> YearSelect()
        {
            int year = DateTime.Now.Year;
            var list = new List<SelectPicker>();
            for (int i= year; i>= year-100; i--)
            {
                list.Add(new SelectPicker()
                {
                    SelectKey = i,
                    SelectValue = i.ToString()
                });
            }
            return list.OrderByDescending(a=>a.SelectKey).ToList();
        }

        /// <summary>
        /// 月份下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectPicker> MonthSelect()
        {
            var list = new List<SelectPicker>();
            for (int i = 12; i >= 1; i--)
            {
                list.Add(new SelectPicker()
                {
                    SelectKey = i,
                    SelectValue = i.ToString()
                });
            }
            return list.OrderByDescending(a => a.SelectKey).ToList();
        }

        /// <summary>
        /// 日期下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<SelectPicker> DaySelect(int year,int month)
        {
            //判断是否闰年
            bool IsRunNian = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;

            //31天的月份
            int[] maxMonth =new int[] { 1, 3, 5, 7, 8, 10, 12 };

            //确定一个月多少天
            int day = 30;
            if(maxMonth.Contains(month))
            {
                day = 31;
            }
            if(month==2)
            {
                //闰年
                if (IsRunNian)
                {
                    day = 29;
                }
                //平年
               else
                {
                    day = 28;
                }
            }

            var list = new List<SelectPicker>();
            for (int i = day; i >= 1; i--)
            {
                list.Add(new SelectPicker()
                {
                    SelectKey = i,
                    SelectValue = i.ToString()
                });
            }
            return list.OrderByDescending(a => a.SelectKey).ToList();
        }

    }
}
