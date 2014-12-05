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
    public class InstructionPageModel
    {
        [Required]
        [Display(Name = "Enter email to create a listing", Prompt = "Enter Email Address")]
        public string Email { get; set; }
    }
}