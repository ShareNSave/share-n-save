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
    [Controls(typeof(InstructionPage))]
    public class InstructionPageController : DBController<InstructionPage>
    {
        public override ActionResult Index()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                var user = HttpContext.User;
                if (user.IsInRole(RoleNames.Administrators) || user.IsInRole(RoleNames.ListingUsers))
                {
                    if (!Request.Url.Query.Contains("add-new-activity"))
                    {
                        if (DataContext.Listings.Include("User").Where(x => x.User != null && x.User.Email == user.Identity.Name).Any())
                            return Redirect("/admin");
                    }
                    //var currentPage = N2.Find.CurrentPage as InstructionPage;
                    return Redirect(CreateListingPageUrl());
                }
            }
            return View(new InstructionPageModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index(InstructionPageModel model)
        {

            if (ModelState.IsValid)
            {
                //MembershipUser user = null;
                
                var user = Membership.GetUser(model.Email);

                //var users = Membership.GetAllUsers().GetEnumerator();
                //while (users.MoveNext())
                //{
                //    var tempUser = (MembershipUser)users.Current;
                //    if (string.Compare(tempUser.Email, model.Email, true) == 0)
                //    {
                //        user = tempUser;
                //        break;
                //    }
                //}
                if (user != null)
                {
                    return Redirect(LoginPageUrl(model));
                }
                else
                {
                    if (DataContext.Listings.Include("User").Where(x => x.User != null && x.User.Email == model.Email).Any())
                        return Redirect("/admin");
                    return Redirect(RegisterPageUrl(model));
                }
            }
            return View(model);
        }

        private string CreateListingPageUrl()
        {
            var currentPage = N2.Find.CurrentPage as InstructionPage;


            if (!string.IsNullOrWhiteSpace(currentPage.CreateListingPage))
            {
                return currentPage.CreateListingPage;
            }
            else
            {
                var createListingPage = N2.Find.Items.Where.Type.Eq(typeof(CreateListingPage)).Select().FirstOrDefault();
                if (createListingPage != null)
                {
                    return createListingPage.Url;
                }
            }
            return "/";
        }

        private string LoginPageUrl(InstructionPageModel model)
        {
            var currentPage = N2.Find.CurrentPage as InstructionPage;

            if (!string.IsNullOrWhiteSpace(currentPage.LoginPage))
            {
                string targetUrl = (DataContext.Listings.Include("User").Where(x => x.User != null && x.User.Email == model.Email).Any()) ? "/admin" : CreateListingPageUrl();
                return string.Format("{0}?returnUrl={1}&email={2}", currentPage.LoginPage, targetUrl, model.Email);
            }

            return "/";
        }

        private string RegisterPageUrl(InstructionPageModel model)
        {
            var currentPage = N2.Find.CurrentPage as InstructionPage;
            if (!string.IsNullOrWhiteSpace(currentPage.RegisterPage))
            {
                return string.Format("{0}?returnUrl={1}&email={2}", currentPage.RegisterPage, CreateListingPageUrl(), model.Email);
            }
            return "/";
        }
    }
}