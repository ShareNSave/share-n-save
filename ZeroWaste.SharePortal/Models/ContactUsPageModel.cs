using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Models.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;

namespace ZeroWaste.SharePortal.Models
{
    public class ContactUsPageModel
    {
        [Required]
        [Display(Name = "Name:", Prompt = "Your name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email:", Prompt = "Your email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Organisation or Group:", Prompt = "Enter Group Name")]
        public string OrganisationOrGroup { get; set; }

        
        [Required]
        [Display(Name = "Street or Postal Address:", Prompt = "Enter Group Address")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "City:", Prompt = "Enter City or Suburb")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State:", Prompt = "Select State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Postcode:", Prompt = "Enter Postcode")]
        public string Postcode { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone:", Prompt = "Enter Phone Number or Group Contact")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Message:", Prompt = "Enter message")]
        public string Message { get; set; }

        [Display(Name = "Receive updates via email")]
        public bool OptIn { get; set; }
    }
}