using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models;
using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Models.Pages;
using ZeroWaste.SharePortal.Services;
using N2;
using N2.Web;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using System.Data.Entity.Spatial;
using System.Web.Security;
using System.Text;
using N2.Web.Mail;
using System.Net.Mail;
using ZeroWaste.SharePortal.Utils;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(CustomThanksPage))]
    public class CustomThanksPageController : DBController<CustomThanksPage>
    {
        public override ActionResult Index()
        {
            return View(CurrentPage);
        }
    }
}