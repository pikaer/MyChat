using Chat.Helper;
using System.Web.Mvc;

namespace Chat
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserCheckAttribute());
        }
    }
}
