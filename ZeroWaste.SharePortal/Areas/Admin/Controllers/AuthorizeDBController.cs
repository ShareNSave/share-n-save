using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Areas.Admin.Utils;
using ZeroWaste.SharePortal.Utils;

namespace ZeroWaste.SharePortal.Areas.Admin.Controllers
{
    [AuthorizeMember(Roles = "Administrators,ListingUsers")]
    public class AuthorizeDBController : AdminDBController
    {
    }
}
