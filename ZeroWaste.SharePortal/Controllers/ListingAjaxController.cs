using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Extensions;
using ZeroWaste.SharePortal.Services;
using ZeroWaste.SharePortal.Utils;
using N2.Web.UI.WebControls;

namespace ZeroWaste.SharePortal.Controllers
{
    public class ListingAjaxController : MvcDBController
    {
        private readonly IGeoLocationService _geoLocationService;
        private readonly ISharePortalSettings _sharePortalSettings;


        public ListingAjaxController(IGeoLocationService geoLocationService, ISharePortalSettings sharePortalSettings)
        {
            _geoLocationService = geoLocationService;
            _sharePortalSettings = sharePortalSettings;
        }

        public JsonResult GetLatAndLngByPostcode(string postcode)
        {
            JsonResult result = new JsonResult();

            double lat = -34.893816, lng = 138.650208;
            if (string.IsNullOrWhiteSpace(postcode))
            {
                Session.Remove("postcode");
            }
            else
            {
                Session["postcode"] = postcode;
                var location = _geoLocationService.GetCentroid(postcode);
                if (location != null)
                {
                    lat = location.Value.Latitude;
                    lng = location.Value.Longitude;
                }
            }

            result.Data = new
            {
                lat,
                lng
            };

            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult GetExploreListings(string searchVal, int? categoryId, int? maxResults, string postcode, List<int> l)
        {
            if (l == null)
            {
                l = new List<int>(0);
            }

            var json = new JsonResult();
            var results = new List<ExploreModel>();

            if (maxResults == null)
            {
                maxResults = _sharePortalSettings.DefaultMaxListingResults;
            }

            var listingsQuery = DataContext.Listings.Where(x => x.IsApproved);

            if (categoryId.HasValue && categoryId.Value != 0)
            {
                // Filter on category ID.
                listingsQuery = listingsQuery.Where(x => x.ListingIcon != null && x.ListingIcon.Category != null && x.ListingIcon.Category.Id == categoryId.Value);
            }

            if (PostcodeValidator.IsValidAustralianPostcode(postcode))
            {
                Session["postcode"] = postcode;

                // Filter by postcode
                var result = _geoLocationService.GetCentroid(postcode);
                if (result != null)
                {
                    // Create DbGeography for spatial query.
                    var location = DbGeography.FromText(string.Format("POINT ({0} {1})", result.Value.Longitude, result.Value.Latitude));
                    
                    // Order by the distance from the postcode centroid. 
                    // We will then take the top 'maxResults' that are closest to the postcode centroid.
                    listingsQuery = listingsQuery.OrderBy( x => x.Location.Distance(location));
                }
            }
            else
            {
                Session.Remove("postcode");
            }

            if (!string.IsNullOrWhiteSpace(searchVal))
            {
                // Filter by serch query.
                listingsQuery = listingsQuery.Where(x => x.Name.ToLower().IndexOf(searchVal) >= 0 || (x.ListingMessage != null && x.ListingMessage.ToLower().IndexOf(searchVal) >= 0));
            }

            
            // Get the results, that have not already been loaded, and in a random order.
            var listings = listingsQuery.Where(x => !l.Contains(x.ListingId));

            if (string.IsNullOrWhiteSpace(searchVal) && string.IsNullOrWhiteSpace(postcode))
            {
                // Only randomise if there is no search query or postcode distance search.
                listings = listings.OrderBy(r => Guid.NewGuid());
            }
            
            var selectedListings = listings.Take(maxResults.Value).ToList();

            if (selectedListings.Count > 0)
            {
                // Count the resutls.
                int totalCount = listingsQuery.Count();
                var existingCount = l.Count;
                bool isShowMore = totalCount > (selectedListings.Count + existingCount);
                results = GetResultsByListing(selectedListings, isShowMore, existingCount);
            }

            json.Data = results.ToList();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        private List<ExploreModel> GetResultsByListing(IReadOnlyCollection<Listing> listings, bool isShowMore, int existingCount)
        {
            var results = new List<ExploreModel>();
            var index = 1 + existingCount;

            foreach (var item in listings)
            {
                string iconName = null, categoryName = null;
                int categoryId = 0;
                if (item.ListingIcon != null && !string.IsNullOrWhiteSpace(item.ListingIcon.Name))
                {
                    iconName = item.ListingIcon.Name;
                    if (item.ListingIcon.Category != null && !string.IsNullOrWhiteSpace(item.ListingIcon.Category.Name))
                    {
                        categoryName = item.ListingIcon.Category.Name;
                        categoryId = item.ListingIcon.Category.Id;
                        switch (categoryId)
                        {
                            case 1: categoryName = "share"; break;
                            case 2: categoryName = "together"; break;
                            case 3: categoryName = "borrow"; break;
                        }
                    }
                }


                if (!string.IsNullOrWhiteSpace(item.WebLink))
                {
                    item.WebLink = item.WebLink.ReplaceQuoteToEmpty();
                    if (!item.WebLink.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                    {
                        item.WebLink = "http://" + item.WebLink;
                    }
                }

                results.Add(new ExploreModel
                {
                    index = index,
                    ListingId = item.ListingId,
                    CategoryId = categoryId,
                    CategoryName = categoryName.ReplaceQuoteToEmpty(),
                    IconName = iconName.ReplaceQuoteToEmpty(),
                    ListingImageLink = ListingHelper.GetImageUrl(item),
                    ListingMessage = item.ListingMessage.ReplaceQuoteToEmpty(),
                    ListingName = item.Name.ReplaceQuoteToEmpty(),
                    ListingWebLink = item.WebLink,
                    IsShowMore = isShowMore,
                    AboutGroup = item.AboutGroup.ReplaceQuoteToEmpty(),
                    Phone = item.Phone.ReplaceQuoteToEmpty(),
                    Email = item.Email.ReplaceQuoteToEmpty(),
                    MapAddress = item.MapAddress.ReplaceQuoteToEmpty()
                });
                index++;
            }

            return results;
        }
    }

    class ExploreModel
    {
        public int index { get; set; }
        public string ListingName { get; set; }
        public string IconName { get; set; }
        public string CategoryName { get; set; }
        public string ListingImageLink { get; set; }
        public string ListingMessage { get; set; }
        public string ListingWebLink { get; set; }
        public bool IsShowMore { get; set; }
        public string AboutGroup { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MapAddress { get; set; }
        public int ListingId { get; set; }
        public int CategoryId { get; set; }
    }

    class ListingComparer : IEqualityComparer<Listing>
    {
        public bool Equals(Listing a, Listing b)
        {
            return a.ListingId == b.ListingId;
        }

        public int GetHashCode(Listing obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
