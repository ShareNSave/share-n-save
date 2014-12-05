using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ZeroWaste.SharePortal.Models.Data
{
    [Table("Listing")]
    public class Listing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListingId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [Obsolete("Use the 'Name' property.")]
        public string Group { get; set; }

        [Obsolete("Not part of the listing signup form.")]
        public string GroupAddress { get; set; }

        public bool IsApproved { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        public string PostCode { get; set; }

        public string Phone { get; set; }

        public string AboutGroup { get; set; }

        public string ListingMessage { get; set; }

        public string MapAddress { get; set; }

        public string WebLink { get; set; }

        [Obsolete("Use the 'WebLink' as the single online resource for the listing.")]
        public string FacebookLink { get; set; }

        public string ListingImageLink { get; set; }

        public DbGeography Location { get; set; }

        public UserProfile User { get; set; }

        public virtual ListingIcon ListingIcon { get; set; }
    }
}