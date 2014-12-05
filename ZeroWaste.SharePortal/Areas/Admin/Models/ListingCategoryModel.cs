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
    public class ListingCategoryModel
    {
        public ListingCategoryModel()
        {
        }
        public ListingCategory ListingCategory { get; set; }
    }
}