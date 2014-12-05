using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroWaste.SharePortal.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToLocalTimeZone(this DateTime dt)
        {
            string v = System.Configuration.ConfigurationManager.AppSettings["LocalTimeZone"];
            if (string.IsNullOrWhiteSpace(v))
            {
                v = "Cen. Australia Standard Time";
            }
            if (!string.IsNullOrEmpty(v))
            {
                TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(v);
                return TimeZoneInfo.ConvertTime(dt, zone);
            }

            return dt;
        }
    }
}