using System.Data;
using System.Globalization;
using ZeroWaste.SharePortal.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Areas.Admin.Models;
using System.Data.Entity.Spatial;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Response;
using System.Web.Security;

using ZeroWaste.SharePortal.Services;
using ZeroWaste.SharePortal.Utils;
using Microsoft.Ajax.Utilities;
using N2.Web.UI.WebControls;
using N2.Web.Mail;
using System.Diagnostics;
using N2;
using ZeroWaste.SharePortal.Models;
using System.Net.Mail;

namespace ZeroWaste.SharePortal.Areas.Admin.Controllers
{
    public class ListingManageController : AuthorizeDBController
    {
        private readonly IListingImageService _listingImageService;
        private readonly IMailSender _mailSender;

        public ListingManageController(IListingImageService listingImageService, IMailSender mailSender)
        {
            _listingImageService = listingImageService;
            _mailSender = mailSender;
        }

        public ActionResult Index(string type, int? listingIconId, int? userId)
        {

            var list = new List<Listing>();
            if (userId.HasValue)
            {
                list = DataContext.Listings.Include("User").Where(x => x.User != null && x.User.UserId == userId.Value).OrderBy(x => x.ListingId).ToList();
            }
            else
            {
                if (HttpContext.User.IsInRole(RoleNames.Administrators))
                {
                    if (string.IsNullOrEmpty(type) || string.Compare("all", type, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        list = DataContext.Listings.Include("User").OrderBy(listing => listing.ListingId).ToList();
                    }
                    else if (string.Compare("requireApproval", type, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        list = DataContext.Listings.Include("User").Where(x => !x.IsApproved).OrderBy(listing => listing.ListingId).ToList();

                    }
                }
                else
                {
                    var user = Membership.GetUser();
                    if (user != null)
                    {
                        var userProfile = DataContext.Users.FirstOrDefault(x => x.Email == user.Email);
                        if (userProfile != null)
                        {
                            list = DataContext.Listings.Where(x => x.User.UserId == userProfile.UserId).OrderBy(listing => listing.ListingId).ToList();
                        }
                    }
                }
            }

            if (listingIconId.HasValue)
            {
                list = list.Where(x => x.ListingIcon != null && x.ListingIcon.ListingIconId == listingIconId.Value).ToList();
            }
            return View(list);
        }

        public ActionResult Edit(int id)
        {
            if (id > 0)
            {
                var listing = DataContext.Listings.Include("User").FirstOrDefault(x => x.ListingId == id);
                if (listing == null)
                {
                    return HttpNotFound();
                }

                // Check permissions.
                var isAdmin = HttpContext.User.IsInRole(RoleNames.Administrators);
                if (string.Compare(HttpContext.User.Identity.Name, "admin", true) != 0)
                {
                    var user = Membership.GetUser();
                    var userProfile = DataContext.Users.FirstOrDefault(x => x.Email == user.Email);
                    if (!isAdmin)
                    {
                        if (listing.User != userProfile)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }

                // Create a view model for editing the listing.
                var model = CreateAdministrationListingModel(listing, isAdmin);

                return View(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(AdminListingViewModel model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }

            if (model != null)
            {
                var listing = DataContext.Listings.Include(i => i.User).FirstOrDefault(x => x.ListingId == model.ListingId);
                if (ModelState.IsValid)
                {

                    if (listing != null)
                    {
                        PopulateListing(model, listing);

                        DataContext.Entry(listing).State = EntityState.Modified;

                        try
                        {
                            DataContext.SaveChanges();
                        }
                        catch { }
                    }
                }
                else
                {
                    AddSelectableItems(model, listing);

                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }

            if (id > 0)
            {
                var listing = DataContext.Listings.FirstOrDefault(x => x.ListingId == id);
                if (listing != null)
                {
                    DataContext.Listings.Remove(listing);
                    DataContext.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return HttpNotFound();
        }

        public ActionResult New()
        {
            AdminListingViewModel model = new AdminListingViewModel();

            var isAdmin = HttpContext.User.IsInRole(RoleNames.Administrators);
            model.IsAdmin = isAdmin;

            AddSelectableItems(model, isAdmin);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(AdminListingViewModel model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }

            if (model != null)
            {
                //if (string.IsNullOrWhiteSpace(model.Location))
                //{
                //    ModelState.AddModelError("Location", "Please select a location.");
                //    model.ListingIconItems = GetListingIconItems();
                //    return View(model);
                //}
                //var userProfile = DataContext.Users.FirstOrDefault(x => x.UserId == model.UserId);
                //if (userProfile == null)
                //{
                //    ModelState.AddModelError("CurrentListingUser", "User is required.");
                //}

                if (ModelState.IsValid)
                {
                    var listing = new Listing();
                    PopulateListing(model, listing);

                    try
                    {
                        DataContext.Listings.Add(listing);
                        DataContext.SaveChanges();
                    }
                    catch { }
                }
                else
                {
                    AddSelectableItems(model);
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        private void SendingApproveNotificationEmail(Listing listing)
        {
            var startPage = Find.StartPage as StartPage;
            var emailContent = startPage.EmailMailIntro;
            if (!string.IsNullOrWhiteSpace(emailContent) && !string.IsNullOrWhiteSpace(listing.Email))
            {
                string url = string.Format("{0}://{1}{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/Admin");
                string name = listing.User.Name;
                emailContent = emailContent.Replace("[name]", name);
                emailContent = emailContent.Replace("[url]", string.Format("<a href='{0}'>{0}</a>", url));
                MailMessage mm = new MailMessage(startPage.EmailMailFrom, listing.Email);
                mm.Subject = startPage.EmailMailSubject;
                mm.Body = emailContent;
                mm.IsBodyHtml = true;

                try
                {
                    _mailSender.Send(mm);
                }
                catch
                {
                    try
                    {
                        _mailSender.Send(mm);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Error sending email: {0}", e);
                    }
                }
            }
        }


        private void AddSelectableItems(AdminListingViewModel model, Listing listing = null)
        {
            AddSelectableItems(model, HttpContext.User.IsInRole(RoleNames.Administrators), listing);
        }

        private void AddSelectableItems(AdminListingViewModel model, bool isAdmin, Listing listing = null)
        {
            if (isAdmin)
            {
                model.Users = DataContext.Users.ToList().Select(profile => new SelectListItem
                {
                    Value = profile.UserId.ToString(),
                    Text = profile.Email
                }).ToList();
            }

            // Is admin is not reserved between requests.
            model.IsAdmin = isAdmin;
            model.ListingIconId = listing != null ? listing.ListingIcon.ListingIconId : 0;
            model.ListingIconItems = GetListingIconItems();
        }

        private List<SelectListItem> GetListingIconItems()
        {
            return (from item in DataContext.ListingIcons.ToList().OrderBy(x => x.ListingIconId)
                    select new SelectListItem
                    {
                        Value = item.ListingIconId.ToString(),
                        Text = item.Name
                    }).ToList();
        }

        private AdminListingViewModel CreateAdministrationListingModel(Listing listing, bool isAdmin)
        {
            if (listing == null)
                return null;

            var model = new AdminListingViewModel
            {
                ListingId = listing.ListingId,
                ListingImageLink = ListingHelper.GetImageUrl(listing),
                IsApproved = listing.IsApproved,
                IsAdmin = isAdmin,
                AboutGroup = listing.AboutGroup,
                City = listing.City,
                CurrentListingUser = listing.User,
                Email = listing.Email,
                //FacebookLink = listing.FacebookLink,
                OrganisationOrGroup = listing.Name,
                PhoneNumber = listing.Phone,
                Postcode = listing.PostCode,
                WebLink = listing.WebLink,
                ListingMessage = listing.ListingMessage,
                MapLocation = listing.MapAddress
            };

            if (listing.Location != null)
            {
                model.Location = listing.Location.Latitude + "," + listing.Location.Longitude;
            }

            if (listing.User != null)
            {
                model.CurrentListingUser = listing.User;
                model.UserId = listing.User.UserId;
            }

            AddSelectableItems(model, isAdmin, listing);

            return model;
        }

        private void PopulateListing(AdminListingViewModel model, Listing listing)
        {
            listing.AboutGroup = model.AboutGroup;
            listing.City = model.City;
            listing.Email = model.Email;
            listing.Group = model.OrganisationOrGroup;
            //listing.GroupAddress = model.Listing.GroupAddress;
            listing.Name = model.OrganisationOrGroup;
            listing.Phone = model.PhoneNumber;
            listing.PostCode = model.Postcode;
            //listing.State = model.State;
            //listing.FacebookLink = model.FacebookLink;
            listing.IsApproved = model.IsApproved;
            listing.WebLink = model.WebLink;
            listing.ListingMessage = model.ListingMessage;
            listing.MapAddress = model.MapLocation;

            var listingIcon = DataContext.ListingIcons.FirstOrDefault(x => x.ListingIconId == model.ListingIconId);
            if (listingIcon != null)
            {
                listing.ListingIcon = listingIcon;
            }

            if (listing.User == null || listing.User.UserId != model.UserId)
            {
                var profile = DataContext.Users.Include(i => i.Listings).SingleOrDefault(x => x.UserId == model.UserId);
                listing.User = profile;
            }

            string[] localtions = model.Location.Split(',');
            DbGeography location = DbGeography.FromText(string.Format("POINT ({0} {1})", localtions[1], localtions[0]));
            listing.Location = location;

            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var imageUrl = _listingImageService.SaveImage(model.Image);
                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    listing.ListingImageLink = imageUrl;
                }
            }

            if (listing.IsApproved)
            {
                try
                {
                    SendingApproveNotificationEmail(listing);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Sending approve notification email error: {0}", e);
                }
            }
        }
    }
}
