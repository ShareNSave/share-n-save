using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZeroWaste.SharePortal.Models.Data
{
    [Table("ResetPasswordLog")]
    public class ResetPasswordLog
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
    }
}