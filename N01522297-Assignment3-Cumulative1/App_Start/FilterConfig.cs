using System.Web;
using System.Web.Mvc;

namespace N01522297_Assignment3_Cumulative1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
