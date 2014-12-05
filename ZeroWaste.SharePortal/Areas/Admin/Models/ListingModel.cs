using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models;
using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Models.Pages;
using ZeroWaste.SharePortal.Utils;

namespace ZeroWaste.SharePortal.Areas.Admin.Models
{
    public class AdminListingViewModel : ListingViewModel
    {
        public bool IsAdmin { get; set; }
        public int UserId { get; set; }
        public UserProfile CurrentListingUser { get; set; }
        public List<SelectListItem> Users { get; set; }

        [Required]
        public string Location { get; set; }
        public bool IsApproved { get; set; }
        public string ListingImageLink { get; set; }

        // Force override until we can get the main site page to use select list items as well.
        public new List<SelectListItem> ListingIconItems { get; set; }
    }

    public class ListingModel
    {
        public ListingModel()
        {
            Listing = new Listing();
            ListingIcon = new ListingIcon();
        }
        public int ListingIconId { get; set; }
        public Listing Listing { get; set; }
        public ListingIcon ListingIcon { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public UserProfile CurrentListingUser { get; set; }
        public string Location { get; set; }
        public string ListingImageLink
        {
            get
            {
                return ListingHelper.GetImageUrl(Listing);
            }
        }

        public IEnumerable<SelectListItem> States
        {
            get
            {
                yield return new SelectListItem { Text = "SA", Value = "SA" };
                yield return new SelectListItem { Text = "WA", Value = "WA" };
                yield return new SelectListItem { Text = "NT", Value = "NT" };
                yield return new SelectListItem { Text = "QLD", Value = "QLD" };
                yield return new SelectListItem { Text = "NSW", Value = "NSW" };
                yield return new SelectListItem { Text = "VIC", Value = "VIC" };
                yield return new SelectListItem { Text = "TAS", Value = "TAS" };
                yield return new SelectListItem { Text = "ACT", Value = "ACT" };
            }
        }

        public List<SelectListItem> ListingIconItems { get; set; }
        public bool IsAdmin { get; set; }
        public List<SelectListItem> Users { get; set; }
        public int UserId { get; set; }
    }
}