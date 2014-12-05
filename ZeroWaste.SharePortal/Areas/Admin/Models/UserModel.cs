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

namespace ZeroWaste.SharePortal.Areas.Admin.Models
{
    public class UserModel
    {
        public UserModel()
        {

        }
        public UserProfile User { get; set; }
        public List<UserProfile> Users { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public IList<Listing> Listings { get; set; }

        public string State { get; set; }

        public IList<SelectListItem> States
        {
            get
            {
                IList<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem { Text = "SA", Value = "SA" });
                list.Add(new SelectListItem { Text = "WA", Value = "WA" });
                list.Add(new SelectListItem { Text = "NT", Value = "NT" });
                list.Add(new SelectListItem { Text = "QLD", Value = "QLD" });
                list.Add(new SelectListItem { Text = "NSW", Value = "NSW" });
                list.Add(new SelectListItem { Text = "VIC", Value = "VIC" });
                list.Add(new SelectListItem { Text = "TAS", Value = "TAS" });
                list.Add(new SelectListItem { Text = "ACT", Value = "ACT" });
                return list;
            }
        }
    }
}