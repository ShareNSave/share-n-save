using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroWaste.SharePortal.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceQuoteToEmpty(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            return str.Replace("\"", "");
        }
    }
}