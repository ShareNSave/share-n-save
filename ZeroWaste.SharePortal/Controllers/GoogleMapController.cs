using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Services;
using System.Data.Entity.Spatial;
using ZeroWaste.SharePortal.Utils;
using Microsoft.SqlServer.Types;
using System.Text;
using ZeroWaste.SharePortal.Extensions;

namespace ZeroWaste.SharePortal.Controllers
{
    public class GoogleMapController : Controller
    {
        private readonly ISharePortalSettings _sharePortalSettings;

        public GoogleMapController(ISharePortalSettings sharePortalSettings)
        {
            _sharePortalSettings = sharePortalSettings;
        }

        public ActionResult Index()
        {
#if DEBUG
            double lat = -34.893816;
            double lng = 138.650208;

            using (var ctx = new ZeroWasteData())
            {
                foreach (var item in ctx.Listings)
                {
                    lat = lat - 0.100505;
                    lng = lng - 0.100505;
                    DbGeography location = DbGeography.FromText(string.Format("POINT ({0} {1})", lng, lat));
                    item.Location = location;
                }
                ctx.SaveChanges();
            }
#endif

            return null;
        }

        /// <summary>
        /// Gets the markers within a defined bounds.
        /// </summary>
        /// <param name="top">The top of the bounds to query.</param>
        /// <param name="right">The right of the bounds to query.</param>
        /// <param name="bottom">The bottom of the bounds to query.</param>
        /// <param name="left">The left of the bounds to query.</param>
        /// <param name="categoryId">The category of the listings to query.</param>
        /// <returns></returns>
        public JsonResult GetMakers(double top, double right, double bottom, double left, int? categoryId)
        {
            var result = new JsonResult();

            // Extiror ring of a polygon defined in a counter clockwise direction.
            var wellKnownText = string.Format("POLYGON(({0} {1}, {0} {3}, {2} {3}, {2} {1}, {0} {1}))", left, top, right, bottom);

            var sqlGeom = SqlGeometry.STGeomFromText(new SqlChars(new SqlString(wellKnownText)), 4326);
            var absLeft = Math.Abs(left);
            var absRight = Math.Abs(right);
            var width = Math.Max(absLeft, absRight) - Math.Min(absLeft, absRight);
            
            // Buffer by 10% of the width.
            sqlGeom = sqlGeom.STBuffer(width*0.1);

            wellKnownText = sqlGeom.STAsText().ToSqlString().ToString();
            var bounds = DbGeography.FromText(wellKnownText);
            
            var maps = new List<MapData>();

            using (var ctx = new ZeroWasteData())
            {
                var listingsQuery  = ctx.Listings.Where(x => x.IsApproved && x.Location.Intersects(bounds) && x.ListingIcon != null && x.ListingIcon.Category != null);

                if (categoryId.HasValue && categoryId.Value != 0)
                {
                     listingsQuery = listingsQuery.Where(x => x.ListingIcon.Category.Id == categoryId.Value);
                }

                //if (!string.IsNullOrWhiteSpace(searchVal))
                //{
                //    listingsQuery = listingsQuery.Where(x => x.Name.ToLower().IndexOf(searchVal.ToLower()) >= 0 || (x.ListingMessage != null && x.ListingMessage.ToLower().IndexOf(searchVal.ToLower()) >= 0));
                //}

                List<Listing> listings = listingsQuery.ToList();

                if (listings.Count > 0)
                {
                    //int index = 0;
                    foreach (var item in listings)
                    {
                        //index++;
                        string listingIcon = null;
                        if (item.ListingIcon != null)
                        {
                            listingIcon = item.ListingIcon.Name;
                        }
                        string icon = string.Format("/content/images/marker/{0}-map.png", listingIcon);
                        if (string.IsNullOrWhiteSpace(listingIcon))
                        {
                            icon = "/content/images/marker/google_map_marker.png";
                        }
                        
                        var content = ListingHelper.CreatePopupHtmlContent(item);

                        maps.Add(new MapData
                        {
                            title = item.Name.Trim(),
                            description = string.Empty,
                            latitude = item.Location.Latitude.ToString(),
                            longitude = item.Location.Longitude.ToString(),
                            icon = icon,
                            pop_content = content,
                        });

                    }
                }
            }

            result.Data = maps;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return result;
        }

    }

    public class MapData
    {
        public string title { get; set; }
        public string description { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string icon { get; set; }
        public string pop_content { get; set; }
    }
}
