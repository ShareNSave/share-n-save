using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Models.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroWaste.SharePortal.Models
{
    public class ExploreListingModel
    {
        public ExploreListingsPart CurrentItem { get; set; }
        public List<Listing> List { get; set; }
        public int CurrentResultNumber { get; set; }
        public bool IsShowMore { get; set; }
        public int MaxResults { get; set; }
    }
}