using System;
using System.Text;
using System.Web.Mvc.Html;


namespace ZeroWaste.SharePortal.Areas.Admin.Extensions
{
    public static class MenuExtensions
    {
        public static string MenuItem(this System.Web.Mvc.HtmlHelper helper, string linkText, string pageTitle, string actionName, string controllerName)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];
            currentActionName = pageTitle;

            var sb = new StringBuilder();

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase))
                sb.Append("<li class=\"selected\">");
            else
                sb.Append("<li>");

            //sb.Append(helper.ActionLink(linkText, actionName, controllerName));
            sb.Append("<a href=\"/" + controllerName + "/" + actionName + "\">" + linkText + "</a>");
            sb.Append("</li>");
            return sb.ToString();
        }
    }
}
