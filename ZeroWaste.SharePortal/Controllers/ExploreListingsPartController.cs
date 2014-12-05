
using System;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models.Parts;
using ZeroWaste.SharePortal.Services;
using ZeroWaste.SharePortal.Utils;
using N2.Web;
using ZeroWaste.SharePortal.Models;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(ExploreListingsPart))]
    public class ExploreListingsPartController : DBController<ExploreListingsPart>
    {
        private readonly IGeoLocationService _geoLocationService;
        private readonly ISharePortalSettings _sharePortalSettings;
        private readonly IExploreListingSettings _exploreListingSettings;

        public ExploreListingsPartController(IGeoLocationService geoLocationService, ISharePortalSettings sharePortalSettings, IExploreListingSettings exploreListingSettings)
        {
            _geoLocationService = geoLocationService;
            _sharePortalSettings = sharePortalSettings;
            _exploreListingSettings = exploreListingSettings;
        }

        public override ActionResult Index()
        {
            var queryable = DataContext.Listings.Where(x => x.IsApproved);
            var postcode = Session["postcode"] as string;
            if (PostcodeValidator.IsValidAustralianPostcode(postcode))
            {
                var result = _geoLocationService.GetCentroid(postcode);
                if (result != null)
                {
                    // Create DbGeography for spatial query.
                    var location = DbGeography.FromText(string.Format("POINT ({0} {1})", result.Value.Longitude, result.Value.Latitude));
                    // Order by the distance from the postcode centroid. 
                    // We will then take the top 'maxResults' that are closest to the postcode centroid.
                    if (_exploreListingSettings.EnableExploreDistanceSearch)
                    {
                        var distance = _exploreListingSettings.DefaultMaxListingDistances;
                        if (distance >= 0)
                        {
                            queryable = queryable.Where(x => x.Location.Distance(location) <= distance);
                        }
                    }
                    queryable = queryable.OrderBy(x => x.Location.Distance(location));
                }
            }
            else
            {
                // Randomize the results
                queryable = queryable.OrderBy(x => Guid.NewGuid());
            }

            var maxResults = _sharePortalSettings.DefaultMaxListingResults;
            var list = queryable.Take(maxResults).ToList();

            var model = new ExploreListingModel
            {
                CurrentItem = CurrentItem,
                List = list,
                CurrentResultNumber = list.Count,
                MaxResults = maxResults
            };

            model.IsShowMore = maxResults <= list.Count;

            return PartialView(model);
        }
    }
}
