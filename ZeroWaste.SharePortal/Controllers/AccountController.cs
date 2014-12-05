using System;
using System.Web.Mvc;
using System.Web.Security;
using ZeroWaste.SharePortal.Models;
using ZeroWaste.SharePortal.Models.Data;
using ZeroWaste.SharePortal.Utils;
using N2;
using ZeroWaste.SharePortal.Extensions;
using ZeroWaste.SharePortal.Services;
using System.Net.Mail;
using System.Linq;

namespace ZeroWaste.SharePortal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ISendEmail _sendEmail;
        public AccountController(ISendEmail sendEmail)
        {
            _sendEmail = sendEmail;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        //#if !DEBUG
        //        [RequireHttps]
        //#endif
        public ActionResult Login(string returnUrl, string email)
        {
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel();
            model.Username = email;
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return Redirect(Find.StartPage.Url);
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        //#if !DEBUG
        //        [RequireHttps]
        //#endif
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Username, model.Password) && Roles.IsUserInRole(model.Username, RoleNames.ListingUsers))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        //#if !DEBUG
        //        [RequireHttps]
        //#endif
        public ActionResult Register(string returnUrl, string email)
        {
            RegisterViewModel model = new RegisterViewModel();
            model.Email = email;
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //#if !DEBUG
        //        [RequireHttps]
        //#endif
        public ActionResult Register(RegisterViewModel model)
        {
            model.Username = model.Email;
            if (ModelState.IsValid)
            {
                try
                {
                    //var allUsers = Membership.GetAllUsers().GetEnumerator();
                    //while (allUsers.MoveNext())
                    //{
                    //    var tempUser = (MembershipUser)allUsers.Current;
                        
                    //    if (string.Compare(tempUser.Email, model.Email, true) == 0)
                    //    {
                    //        ModelState.AddModelError("Email", "The email address already exists.");
                    //    }
                    //    if (string.Compare(tempUser.UserName, model.Username, true) == 0)
                    //    {
                    //        ModelState.AddModelError("Username", "The username already exists.");
                    //    }
                    //    if (!ModelState.IsValid)
                    //    {
                    //        return View(model);
                    //    }
                    //}
                    var cu = Membership.GetUser(model.Email);
                    if (cu != null)
                    {
                        ModelState.AddModelError("Email", "The email is already exists.");
                    }
                     if (!ModelState.IsValid)
                     {
                         return View(model);
                     }

                    var membershipUser = Membership.CreateUser(model.Username, model.Password, model.Email);
                    var validated = Membership.ValidateUser(model.Username, model.Password);
                    if (validated)
                    {
                        try
                        {
                            using (var context = new ZeroWasteData())
                            {
                                //var accout = context.Accouts.Single(accout1 => accout1.UserName == model.Username);
                                var user = CreateNewUserProfile(model);
                                context.Users.Add(user);
                                context.SaveChanges();

                                Roles.AddUserToRole(membershipUser.UserName, RoleNames.ListingUsers);
                            }

                            FormsAuthentication.SetAuthCookie(model.Username, false);
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError("", "An error occured trying to create the account.");
                            Membership.DeleteUser(model.Username);
                            return View(model);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return Redirect(Find.StartPage.Url);
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(Guid userId)
        {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                UserId = userId,
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ForgotPasswordViewModel model)
        {
            if (model != null && !string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                if (string.Compare(model.NewPassword, model.ConfirmPassword, true) != 0)
                {
                    ModelState.AddModelError("ConfirmPassword", "The two passwords do no match.");
                }
            }
            else
            {
                ModelState.AddModelError("ConfirmPassword", "The two passwords can not be null.");
            }
            if (ModelState.IsValid)
            {
                var startPage = N2.Find.StartPage as StartPage;
                using (var ctx = new ZeroWasteData())
                {
                    var resetPasswordLog = ctx.ResetPasswordLog.Where(x => x.Id == model.UserId).FirstOrDefault();
                    if (resetPasswordLog != null)
                    {
                        var user = Membership.GetUser(resetPasswordLog.Email);
                        if (user != null)
                        {
                            try
                            {
                                var oldPassword = user.ResetPassword();
                                if (user.ChangePassword(oldPassword, model.NewPassword))
                                {
                                    var returnUrl = startPage.ResetPasswordSuccessPage;
                                    if (string.IsNullOrWhiteSpace(returnUrl))
                                    {
                                        returnUrl = "/";
                                    }
                                    return Redirect(returnUrl);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var email = model.Email;
            if (model != null && !string.IsNullOrWhiteSpace(email))
            {
                MembershipUser user = null;
                var users = Membership.GetAllUsers().GetEnumerator();
                while (users.MoveNext())
                {
                    var tempUser = (MembershipUser)users.Current;
                    if (string.Compare(tempUser.Email, email, true) == 0)
                    {
                        user = tempUser;
                        break;
                    }
                }
                if (user == null)
                {
                    ModelState.AddModelError("Email", "User does not exists.");
                }
                else
                {
                    ResetPasswordLog resetPasswordLog = new ResetPasswordLog()
                    {
                        Id = Guid.NewGuid(),
                        Email = user.UserName,
                        CreateBy = "Admin",
                        CreateAt = DateTime.Now.ToLocalTimeZone(),
                    };
                    using (var ctx = new ZeroWasteData())
                    {
                        bool success = false;
                        try
                        {
                            ctx.ResetPasswordLog.Add(resetPasswordLog);
                            ctx.SaveChanges();
                            success = true;
                        }
                        catch { success = false; }
                        if (success)
                        {
                            var startPage = N2.Find.StartPage as StartPage;
                            var returnUrl = startPage.SendResetPasswordEmailSuccessPage;
                            if (startPage != null && !string.IsNullOrWhiteSpace(startPage.ResetPasswordEmailContent))
                            {
                                var emailContent = startPage.ResetPasswordEmailContent;
                                emailContent = emailContent.Replace("[username]", user.UserName);
                                string url = string.Format("{0}://{1}/Account/ResetPassword?UserId={2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, resetPasswordLog.Id);
                                emailContent = emailContent.Replace("[url]", url);
                                MailMessage mm = new MailMessage();
                                mm.To.Add(user.Email);
                                mm.Subject = "Reset Password";
                                mm.IsBodyHtml = true;
                                mm.Body = emailContent;

                                try
                                {
                                    _sendEmail.SendingEmail(mm);
                                    if (string.IsNullOrWhiteSpace(returnUrl))
                                    {
                                        returnUrl = "/";
                                    }
                                    return Redirect(returnUrl);
                                }
                                catch
                                {
                                    try
                                    {
                                        _sendEmail.SendingEmail(mm);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        private UserProfile CreateNewUserProfile(RegisterViewModel registerViewModel)
        {
            return new UserProfile
            {
                Name = registerViewModel.Name,
                Email = registerViewModel.Email,
                OrganisationOrGroup = registerViewModel.OrganisationOrGroup,
                GroupAddress = registerViewModel.GroupAddress,
                City = registerViewModel.City,
                Phone = registerViewModel.Phone,
                Postcode = registerViewModel.Postcode,
                State = GetState(registerViewModel.State),
                Username = registerViewModel.Username,
                AboutGroup = registerViewModel.AboutGroup,
            };
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect(Find.StartPage.Url);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
