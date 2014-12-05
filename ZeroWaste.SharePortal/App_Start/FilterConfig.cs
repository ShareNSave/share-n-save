using System.Web;
using System.Web.Mvc;

namespace ZeroWaste.SharePortal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}