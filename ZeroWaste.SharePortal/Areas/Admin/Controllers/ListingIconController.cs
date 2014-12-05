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
using N2.Web.UI.WebControls;

namespace ZeroWaste.SharePortal.Areas.Admin.Controllers
{
    public class ListingIconController : AuthorizeDBController
    {
        public ActionResult Index(int? categoryId)
        {
            List<ListingIcon> list = new List<ListingIcon>();
            if (categoryId.HasValue)
            {
                list = DataContext.ListingIcons.Where(x => x.Category.Id == categoryId.Value).ToList();
            }
            else
            {
                list = DataContext.ListingIcons.ToList();
            }

            return View(list);
        }

        public ActionResult Edit(int id)
        {
            ListingIconModel model = new ListingIconModel();

            if (id > 0)
            {
                model.ListingIcon = DataContext.ListingIcons.FirstOrDefault(x => x.ListingIconId == id);
            }

            model.ListingCategories = GetListingCategories();

            return View(model);
        }

        private List<SelectListItem> GetListingCategories()
        {
            return (from item in DataContext.ListingCategories.OrderBy(x => x.Id).ToList()
                    select new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name
                    }).ToList();
        }

        [HttpPost]
        public ActionResult Edit(ListingIconModel model)
        {
            if (model != null)
            {
                var listingIcon = DataContext.ListingIcons.FirstOrDefault(x => x.ListingIconId == model.ListingIcon.ListingIconId);

                if (listingIcon != null)
                {
                    listingIcon.Name = model.ListingIcon.Name;
                    listingIcon.IconPath = model.ListingIcon.IconPath;
                    listingIcon.Description = model.ListingIcon.Description;

                    var category = DataContext.ListingCategories.FirstOrDefault(x => x.Id == model.CategoryId);
                    if (category != null)
                    {
                        listingIcon.Category = category;
                    }
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
            ListingIconModel model = new ListingIconModel();

            model.ListingCategories = GetListingCategories();

            return View(model);
        }
        public ActionResult DeleteListingIcon(int id)
        {

            ListingIcon icon = DataContext.ListingIcons.FirstOrDefault(m => m.ListingIconId == id);
            if (icon != null)
            {
                try
                {
                    DataContext.ListingIcons.Remove(icon);
                }
                catch
                {

                }
            }

            return RedirectToAction("Index");
        }

        private void ValidateListingIcon(ListingIconModel model)
        {
            ModelState.Clear();
            #region Validate
            if (string.IsNullOrEmpty(model.ListingIcon.Name))
            {
                ModelState.AddModelError("ListingIcon.Name", "The name is required.");
            }

            if (model.CategoryId<=0)
            {
                ModelState.AddModelError("CategoryId", "Please select category.");
            }

            #endregion
        }

        [HttpPost]
        public ActionResult New(ListingIconModel model, HttpPostedFileBase imageFile)
        {
            if (model != null)
            {
                ValidateListingIcon(model);
                if (!ModelState.IsValid)
                {
                    model.ListingCategories = GetListingCategories();
                    return View(model);
                }
                #region Validate
                #endregion

                #region Upload File
                try
                {
                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        if (hpf.ContentLength == 0)
                            continue;
                        string iconFolder = "~/Content/images/icons/";

                        if (!System.IO.Directory.Exists(Server.MapPath(iconFolder)))
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(iconFolder));
                        }
                        string folder = Server.MapPath(iconFolder) ;
                        string geneCode = "";
                        while (System.IO.File.Exists(folder + geneCode + hpf.FileName))
                        {
                            geneCode = new Random().Next(1000, 9999).ToString();
                        }
                        hpf.SaveAs(folder + geneCode + hpf.FileName);
                        model.ListingIcon.IconPath = iconFolder+geneCode + hpf.FileName;
                    }
                }
                catch
                {
                    model.ListingCategories = GetListingCategories();
                    ModelState.AddModelError("ListingIcon.IconPath", "Icon Image upload field.");
                    return View(model);
                }
                #endregion
                ListingIcon listingIcon = new ListingIcon()
                {
                    Name = model.ListingIcon.Name,
                    IconPath = model.ListingIcon.IconPath,
                    Description = model.ListingIcon.Description
                };

                var category = DataContext.ListingCategories.FirstOrDefault(x => x.Id == model.CategoryId);
                if (category != null)
                {
                    listingIcon.Category = category;
                }

                try
                {
                    DataContext.ListingIcons.Add(listingIcon);
                    DataContext.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }

            return RedirectToAction("Index");
        }
    }
}
