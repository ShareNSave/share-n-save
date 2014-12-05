using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Models.Pages;
using ZeroWaste.SharePortal.Utils;

namespace ZeroWaste.SharePortal.Models
{
    public class ListingViewModel
    {
        public int ListingId { get; set; }
        // Form fields
        //[Required]
        //[Display(Name = "Name", Prompt = "Your name")]
        //[Description("")]
        //public string Name { get; set; }

        //[Required]
        [Display(Name = "Where can the public email you:", Prompt = "The email address you want to appear on the website")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Tell us the name of your group:", Prompt = "The name you want to appear on the website")]
        public string OrganisationOrGroup { get; set; }

        [Required]
        [Display(Name = "Suburb:", Prompt = "Enter Suburb")]
        public string City { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "About your group - tell us what you do:", Prompt = "A brief message Max 100 words")]
        public string AboutGroup { get; set; }

        [Required]
        [Display(Name = "Postcode:", Prompt = "Enter Postcode")]
        public string Postcode { get; set; }
        //[Required]
        //[Display(Name = "Group Address", Prompt = "Enter Group Address")]
        //public string Street { get; set; }


        //[Required]
        //[Display(Name = "State", Prompt = "Select State")]
        //public StateOptions? State { get; set; }

        //[Required]
        [Phone]
        [Display(Name = "Got a phone number you’d like the public to call? Add it here:", Prompt = "Enter Phone Number")]
        public string PhoneNumber { get; set; }


        [Required]
        [Display(Name = "Listing Category", Prompt = "Select Listing Category")]
        public int ListingIconId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Are there regular days you operate? First Saturday of the month? Or are there opening hours? Tell us here:", Prompt = "The days and times your group is available.")]
        public string ListingMessage { get; set; }

        [Required]
        [Display(Name = "Street address:", Prompt = "We will map your location using these address details.")]
        public string MapLocation { get; set; }

        [Obsolete("Obsolete")]
        [Url]
        [Display(Name = "Got a Facebook or social media page? Of course you do, pop that in here. (If not it won’t take long to set one up and it’s free - you can come back any time to update this listing)", Prompt = "Best to copy from your browser address bar.")]
        public string FacebookLink { get; set; }

        [Url]
        [Display(Name = "Got a Facebook, social media site or website? Pop the web address in here. Make sure it is the link that is the most up-to-date. (If you don't have anything it doesn't take long to set one up and It's free - you can come back and update this listing.)", Prompt = "Best to copy from your browser address bar.")]
        public string WebLink { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public List<ListingIcon> ListingIconItems { get; set; }
    }
}