using ZeroWaste.SharePortal.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Areas.Admin.Models;
using System.Data.Entity.Spatial;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Response;
using System.Data.Entity.Validation;

namespace ZeroWaste.SharePortal.Areas.Admin.Controllers
{
    public class ListingCategoryController : AuthorizeDBController
    {
        public ActionResult Index()
        {
            List<ListingCategory> list = new List<ListingCategory>();

            list = DataContext.ListingCategories.ToList();

            return View(list);
        }

        public ActionResult Edit(int id)
        {
            ListingCategoryModel model = new ListingCategoryModel();

            if (id > 0)
            {
                model.ListingCategory = DataContext.ListingCategories.FirstOrDefault(x => x.Id == id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ListingCategoryModel model)
        {
            if (model != null)
            {
                var listingCategory = DataContext.ListingCategories.FirstOrDefault(x => x.Id == model.ListingCategory.Id);

                if (listingCategory != null)
                {
                    listingCategory.Name = model.ListingCategory.Name;
                }

                try
                {
                    DataContext.SaveChanges();
                }
                catch { }
            }

            return RedirectToAction("Index");
        }

        public ActionResult New()
        {
            ListingCategoryModel model = new ListingCategoryModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult New(ListingCategoryModel model)
        {
            if (model != null)
            {
                ListingCategory listingCategory = new ListingCategory()
{
    Name = model.ListingCategory.Name
};
                try
                {
                    DataContext.ListingCategories.Add(listingCategory);
                    DataContext.SaveChanges();
                }
                catch { }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteListingCategory(int id)
        {

            ListingCategory category = DataContext.ListingCategories.FirstOrDefault(m => m.Id == id);
            if (category != null)
            {
                try
                {
                    DataContext.ListingCategories.Remove(category);
                    DataContext.SaveChanges();
                }
                catch(Exception ex)
                {

                }
            }

            return RedirectToAction("Index");
        }
    }
}
