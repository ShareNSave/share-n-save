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
using ZeroWaste.SharePortal.Models;
using System.Web.Security;
using ZeroWaste.SharePortal.Utils;

namespace ZeroWaste.SharePortal.Areas.Admin.Controllers
{
    public class UserController : AuthorizeDBController
    {
        public ActionResult Index()
        {
            List<UserProfile> list = DataContext.Users.ToList();

            return View(list);
        }

        public ActionResult Edit(int id)
        {

            UserModel model = new UserModel();

            if (id > 0)
            {
                model.User = DataContext.Users.Include("Listings").FirstOrDefault(x => x.UserId == id);
            }

            if (model.User != null && model.User.Listings != null && model.User.Listings.Count > 0)
            {
                model.Listings = model.User.Listings.ToList();
            }
            else
            {
                model.Listings = new List<Listing>();
            }

            model.State = model.User.State.ToString();
            return View(model);
        }
        private State GetState(string state)
        {
            switch (state)
            {
                case "SA": return State.SA;
                case "WA": return State.WA;
                case "NT": return State.NT;
                case "QLD": return State.QLD;
                case "NSW": return State.NSW;
                case "VIC": return State.VIC;
                case "TAS": return State.TAS;
                case "ACT": return State.ACT;
            }
            return State.SA;
        }
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            ValidateUserProfile(model);

            if (ModelState.IsValid)
            {

                var oldUser = DataContext.Users.FirstOrDefault(x => x.UserId == model.User.UserId);

                if (oldUser != null)
                {
                    oldUser.Name = model.User.Name;
                    oldUser.Username = model.User.Username;
                    oldUser.OrganisationOrGroup = model.User.OrganisationOrGroup;
                    oldUser.GroupAddress = model.User.GroupAddress;
                    oldUser.City = model.User.City;
                    oldUser.State = GetState(model.State);
                    oldUser.Postcode = model.User.Postcode;
                    oldUser.Phone = model.User.Phone;
                    oldUser.AboutGroup = model.User.AboutGroup;
                    if (oldUser.Email != model.User.Email)
                    {


                        //var userName = Membership.GetUserNameByEmail(oldUser.Email);
                        var cu = Membership.GetUser(oldUser.Username);
                        if (cu != null)
                        {
                            cu.Email = model.User.Email;
                            Membership.UpdateUser(cu);

                        }

                    }
                    oldUser.Email = model.User.Email;
                    DataContext.SaveChanges();
                }

            }
            else
            {
                return View(model);
            }
            return Redirect("/Admin/User/Details?userId=" + model.User.UserId);
        }

        public ActionResult Details(int userId)
        {
            UserModel model = new UserModel();

            if (userId > 0)
            {
                model.User = DataContext.Users.FirstOrDefault(x => x.UserId == userId);
            }

            return View(model);
        }


        public ActionResult New()
        {
            UserModel model = new UserModel();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var userProfile = DataContext.Users.FirstOrDefault(x => x.UserId == id);
            if (userProfile != null && !string.IsNullOrWhiteSpace(userProfile.Username))
            {
                try
                {
                    Membership.DeleteUser(userProfile.Username, true);
                }
                catch { }
            }

            try
            {
                var listings = DataContext.Listings.Where(x => x.User.UserId == userProfile.UserId);
                foreach (var item in listings)
                {
                    item.User = null;
                }
                DataContext.SaveChanges();
            }
            catch { }

            try
            {
                DataContext.Users.Remove(userProfile);
                DataContext.SaveChanges();
            }
            catch { }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult New(UserModel model)
        {
            ValidateUserProfile(model);
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "The password is required.");
                }
                else if (string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "The confirmPassword is required.");

                }
                else if (model.ConfirmPassword != model.Password)
                {
                    ModelState.AddModelError("ConfirmPassword", "The confirmPassword is not the same as password.");
                }
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                try
                {
                    var allUsers = Membership.GetAllUsers().GetEnumerator();
                    while (allUsers.MoveNext())
                    {
                        var tempUser = (MembershipUser)allUsers.Current;
                        if (String.Compare(tempUser.Email, model.User.Email, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            ModelState.AddModelError("User.Email", "The email address already exists.");
                        }
                        if (String.Compare(tempUser.UserName, model.User.Username, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            ModelState.AddModelError("User.Username", "The username already exists.");
                        }
                        if (!ModelState.IsValid)
                        {
                            return View(model);
                        }
                    }

                    try
                    {
                        var membershipUser = Membership.CreateUser(model.User.Username, model.Password, model.User.Email);
                        using (var context = new ZeroWasteData())
                        {
                            //var accout = context.Accouts.Single(accout1 => accout1.UserName == model.Username);
                            model.User.State = GetState(model.State);
                            context.Users.Add(model.User);
                            context.SaveChanges();

                            Roles.AddUserToRole(membershipUser.UserName, RoleNames.ListingUsers);
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "An error occured trying to create the account.");
                        Membership.DeleteUser(model.User.Username);
                        return View(model);
                    }

                }
                catch (Exception ex)
                {
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }


        private void ValidateUserProfile(UserModel model)
        {
            ModelState.Clear();

            //    public int UserId { get; set; }
            //public string Name { get; set; }
            //public string Email { get; set; }
            //public string Username { get; set; }
            //public string OrganisationOrGroup { get; set; }
            //public string GroupAddress { get; set; }
            //public string City { get; set; }
            //public State State { get; set; }
            //public string Postcode { get; set; }
            //public string Phone { get; set; }
            //public string AboutGroup { get; set; }
            #region Validate
            if (string.IsNullOrEmpty(model.User.Name))
            {
                ModelState.AddModelError("User.Name", "The name is required.");
            }

            if (string.IsNullOrEmpty(model.User.Email))
            {
                ModelState.AddModelError("User.Email", "The email is required.");
            }

            if (string.IsNullOrEmpty(model.User.Username))
            {
                ModelState.AddModelError("User.Username", "The user name is required.");
            }

            if (string.IsNullOrEmpty(model.User.OrganisationOrGroup))
            {
                ModelState.AddModelError("User.OrganisationOrGroup", "The organisation is required.");
            }

            if (string.IsNullOrEmpty(model.User.GroupAddress))
            {
                ModelState.AddModelError("User.GroupAddress", "The group address is required.");
            }

            if (string.IsNullOrEmpty(model.User.City))
            {
                ModelState.AddModelError("User.City", "The city is required.");
            }

            if (string.IsNullOrEmpty(model.User.Phone))
            {
                ModelState.AddModelError("User.Phone", "The phone is required.");
            }

            //if (string.IsNullOrEmpty(model.User.AboutGroup))
            //{
            //    ModelState.AddModelError("User.AboutGroup", "The about group is required.");
            //}



            #endregion
        }


        //public ActionResult SysnUserProfile()
        //{
        //    var listings = DataContext.Listings.ToList();

        //    foreach (var listing in listings)
        //    {
        //        var email = listing.Email;

        //        if (!string.IsNullOrWhiteSpace(email))
        //        {
        //            var user = DataContext.Users.FirstOrDefault(m => m.Email == email);

        //            if (user != null)
        //            {
        //                //if (string.IsNullOrWhiteSpace(user.Name))
        //                //{
        //                //    user.Name = listing.Name;
        //                //}
        //                if (string.IsNullOrWhiteSpace(user.OrganisationOrGroup))
        //                {
        //                    user.OrganisationOrGroup = listing.Group;
        //                }
        //                if (string.IsNullOrWhiteSpace(user.GroupAddress))
        //                {
        //                    user.GroupAddress = listing.GroupAddress;
        //                }
        //                if (string.IsNullOrWhiteSpace(user.City))
        //                {
        //                    user.City = listing.City;
        //                }
        //                user.State = listing.State;
        //                if (string.IsNullOrWhiteSpace(user.Postcode))
        //                {
        //                    user.Postcode = listing.PostCode;
        //                }
        //                if (string.IsNullOrWhiteSpace(user.Phone))
        //                {
        //                    user.Phone = listing.Phone;
        //                }
        //                if (string.IsNullOrWhiteSpace(user.AboutGroup))
        //                {
        //                    user.AboutGroup = listing.AboutGroup;
        //                }
        //                listing.User = user;
        //                var l = DataContext.Listings.FirstOrDefault(m => m.ListingId == listing.ListingId);
        //                if (l != null)
        //                {
        //                    l.User = user;
        //                }

        //                DataContext.SaveChanges();

        //            }
        //            else
        //            {

        //                UserProfile newUser = new UserProfile();
        //                newUser.Name = listing.Name;
        //                newUser.OrganisationOrGroup = listing.Group;
        //                newUser.GroupAddress = listing.GroupAddress;
        //                newUser.City = listing.City;
        //                newUser.Postcode = listing.PostCode;
        //                newUser.Phone = listing.Phone;
        //                newUser.AboutGroup = listing.AboutGroup;
        //                newUser.State = listing.State;
        //                newUser.Email = listing.Email;

        //                newUser = DataContext.Users.Add(newUser);
        //                listing.User = user;
        //                var l = DataContext.Listings.FirstOrDefault(m => m.ListingId == listing.ListingId);
        //                if (l != null)
        //                {
        //                    l.User = user;
        //                }
        //                DataContext.SaveChanges();


        //            }
        //        }

        //    }

        //    return Content("OK");
        //}
    }
}
