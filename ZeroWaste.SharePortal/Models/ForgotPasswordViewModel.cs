using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZeroWaste.SharePortal.Models.Data;

namespace ZeroWaste.SharePortal.Models
{
    public class ForgotPasswordViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}