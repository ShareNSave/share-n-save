using System;
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
using N2.Web.Mail;
using System.Net.Mail;
using System.Diagnostics;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(CreateListingPage))]
    public class CreateListingController : DBController<CreateListingPage>
    {
        private readonly IListingImageService _listingImageService;
        private readonly IMailSender _mailSender;

        public CreateListingController(IListingImageService listingImageService, IMailSender mailSender)
        {
            _listingImageService = listingImageService;
            _mailSender = mailSender;
        }

        public override ActionResult Index()
        {
            //// Initialise the form data property to prevent null references.
            //CurrentItem.Form = new ListingViewModel();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ListingViewModel model = new ListingViewModel();
                model.ListingIconItems = DataContext.ListingIcons.OrderBy(x => x.ListingIconId).ToList();

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = N2.Find.CurrentPage.Url });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ListingViewModel model)
        {
            if (ModelState.IsValid)
            {
                string strName = User.Identity.Name;

                // let's generate a filename to store the file on the server
                var file = model.Image;
                string imageUrl = null;

                // Check if they selected a custom image
                if (file != null && file.ContentLength > 0)
                {
                    // Save it on the server and record the path.
                    imageUrl = _listingImageService.SaveImage(file);
                }

                DbGeography location = null;
                try
                {
                    string targetAddress = String.Concat(model.MapLocation, ", ", model.City, ", ", model.Postcode, " Australia");
                    System.Diagnostics.Trace.WriteLine("Geolocating: {0}", targetAddress);
                    var geocodeRequest = new GeocodingRequest
                    {
                        Address = targetAddress,
                    };

                    var geocode = GoogleMaps.Geocode.Query(geocodeRequest);


                    if (geocode.Status == Status.OK)
                    {
                        var results = geocode.Results.ToArray();

                        var result = results.First();
                        location =
                            DbGeography.FromText(string.Format("POINT ({0} {1})", result.Geometry.Location.Longitude,
                                result.Geometry.Location.Latitude));
                    }
                    else if (geocode.Status == Status.ZERO_RESULTS || geocode.Status == Status.INVALID_REQUEST)
                    {
                        ModelState.AddModelError("MapLocation", "Could not locate the provided address. Perhaps search google map for an exact match?");
                    }
                    else if (geocode.Status == Status.OVER_QUERY_LIMIT)
                    {
                        ModelState.AddModelError("MapLocation", "We currently cannot process the location of your listing (Over query limit).");
                    }
                    else if (geocode.Status == Status.REQUEST_DENIED)
                    {
                        ModelState.AddModelError("MapLocation", "We currently cannot process the location of your listing (Request denied).");
                    }

                    if (!ModelState.IsValid)
                    {
                        // repopulate the listing icons.
                        using (var dataContext = new ZeroWasteData())
                        {
                            model.ListingIconItems = dataContext.ListingIcons.OrderBy(x => x.ListingIconId).ToList();

                            // Send back the error
                            return View(model);
                        }
                    }
                }
                catch
                {
                }

                var tempUser = Membership.GetUser(User.Identity.Name);

                if (tempUser != null)
                {
                    var userProfile = DataContext.Users.FirstOrDefault(x => x.Email == tempUser.Email);
                    if (userProfile != null)
                    {

                        var listing = new Listing
                        {
                            Name = model.OrganisationOrGroup,
                            Email = model.Email,
                            Group = model.OrganisationOrGroup,
                            City = model.City,
                            State = userProfile.State,
                            PostCode = model.Postcode,
                            Phone = model.PhoneNumber,
                            AboutGroup = model.AboutGroup,
                            ListingMessage = model.ListingMessage,
                            WebLink = model.WebLink,
                            //FacebookLink = model.FacebookLink,
                            MapAddress = model.MapLocation,
                            ListingImageLink = imageUrl,
                            IsApproved = false,
                            Location = location,
                            User = userProfile,
                        };

                        var listIcon = DataContext.ListingIcons.FirstOrDefault(x => x.ListingIconId == model.ListingIconId);
                        if (listIcon != null)
                        {
                            listing.ListingIcon = listIcon;
                        }

                        DataContext.Listings.Add(listing);
                        DataContext.SaveChanges();

                        //Sending admin approval email
                        var adminEmailContent = CurrentItem.EmailMailIntro;
                        if (listing.ListingId > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(adminEmailContent))
                            {
                                string approvalLink = string.Format("{0}://{1}{2}{3}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/Admin/ListingManage/Edit/", listing.ListingId);

                                adminEmailContent = adminEmailContent.Replace("[url]", string.Format("<a href='{0}'>{0}</a>", approvalLink));
                                MailMessage mm = new MailMessage(CurrentItem.EmailMailFrom, CurrentItem.EmailMailTo);
                                mm.Subject = CurrentItem.EmailMailSubject;
                                mm.Body = adminEmailContent;
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

                            //Sending public notification email
                            var publicEmailContent = CurrentItem.EmailMailIntroPublic;
                            if (!string.IsNullOrWhiteSpace(publicEmailContent) && listing.User != null && !string.IsNullOrWhiteSpace(listing.User.Email))
                            {
                                string name = listing.User.Name;
                                string mailTo = listing.User.Email;

                                publicEmailContent = publicEmailContent.Replace("[name]", name);
                                MailMessage mm = new MailMessage(CurrentItem.EmailMailFrom, mailTo);
                                mm.Subject = CurrentItem.EmailMailSubject;
                                mm.Body = publicEmailContent;
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
                    }
                }
            }

            // Listing created :)
            var redirectTo = String.IsNullOrEmpty(CurrentItem.SuccessfulListingPage)
                ? Find.StartPage.Url
                : CurrentItem.SuccessfulListingPage;

            return Redirect(redirectTo);
        }
    }
}
