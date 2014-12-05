using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZeroWaste.SharePortal.Models.Data;

namespace ZeroWaste.SharePortal.Models
{
    [DisplayName("Login")]
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    [DisplayName("Register")]
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Name:", Prompt = "Your Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Group or Organisation:", Prompt = "Enter Group Name")]
        public string OrganisationOrGroup { get; set; }

        [Required]
        [Display(Name = "Group Address (For administration):", Prompt = "Enter Group Address")]
        public string GroupAddress { get; set; }

        [Display(Name = "Tell Us About Your Group:", Prompt = "Tell us aboit your group, keeping in mind the criteria on the left.")]
        [DataType(DataType.MultilineText)]
        public string AboutGroup { get; set; }

        [Required]
        [Display(Name = "City/Suburb:", Prompt = "Enter City or Suburb")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State:", Prompt = "Select State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Postcode:", Prompt = "Enter Postcode")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Phone contact (For administration):", Prompt = "Enter Phone Number for Group Contact")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email Adress for Administration:", Prompt = "name@email.com")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Create a Password:", Prompt = "Minimum 8 characters")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password by typing in here again:", Prompt = "Minimum 8 characters")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This is used to show state options in the UI
    /// This is separate from the <see cref="State"/> enum, as you can change
    /// the order of this enum to show a differnt order on the UI, without breaking the database.
    /// As the database is storing an integer, by chagning the order of <see cref="State"/>, you will
    /// change the meaning of the existing records.
    /// </remarks>
    public enum StateOptions
    {
        // Note you can change the UI in a combo box by setting the description.
        [Description("SA")]
        SA,

        WA,

        NT,

        QLD,

        NSW,

        VIC,

        TAS,

        ACT

    }
}