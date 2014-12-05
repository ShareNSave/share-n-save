using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ZeroWaste.SharePortal.Models.Data
{
    [Table("ListingIcon")]
    public class ListingIcon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListingIconId { get; set; }

        public string Description { get; set; }
        [Display(Name = "Category", Prompt = "Category")]
        public string Name { get; set; }
        public string IconPath { get; set; }
        public List<Listing> Listings { get; set; }

        [Required]
        public virtual ListingCategory Category { get; set; }
    }

    [Table("ListingCategory")]
    public class ListingCategory
    {
        public const string ShareAndSwap = "Share & Swap";

        public const string DoThingsTogether = "Do Things Together";

        public const string BorrowThings = "Borrow Things";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}