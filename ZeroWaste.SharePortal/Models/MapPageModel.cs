using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Models.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroWaste.SharePortal.Models
{
    public class MapPageModel
    {
        public MapPage CurrentItem { get; set; }
        public List<Listing> ListingItems { get; set; }
    }
}
