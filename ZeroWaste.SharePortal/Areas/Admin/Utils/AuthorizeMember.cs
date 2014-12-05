using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models.Data;

namespace ZeroWaste.SharePortal.Areas.Admin.Utils
{
    public class AuthorizeMemberAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            using (var DataContext = new ZeroWasteData())
            {
                if (filterContext.IsChildAction) return;
                
                var loginResult = new RedirectResult(
                    string.Format("/Account/Login?ReturnUrl={0}",
                        filterContext.HttpContext.Server.UrlEncode(
                            filterContext.HttpContext.Request.Url.PathAndQuery)));

                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = loginResult;
                    return;
                }


                if (string.Compare(filterContext.HttpContext.User.Identity.Name, "admin", true) == 0)
                {
                    return;
                }

                var httpContext = filterContext.RequestContext.HttpContext;
                string[] roles = Roles.Split(',');
                if (roles != null && roles.Length > 0)
                {
                    bool inRole = false;
                    foreach (var role in roles)
                    {
                        if (httpContext.User.IsInRole(role))
                        {
                            inRole = true;
                        }
                    }
                    if (!inRole)
                    {
                        filterContext.Result = loginResult;
                        return;
                    }
                }
            }
        }
    }
}