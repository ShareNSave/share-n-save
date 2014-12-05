using System.Data.Entity;

namespace ZeroWaste.SharePortal.Models.Data
{
    public class ZeroWasteData : DbContext
    {
        public ZeroWasteData()
            : base("DefaultConnection")
        {
        }

        /// <summary>
        /// Gest or sets the set of listings in zero waste S'n'S.
        /// </summary>
        public DbSet<Listing> Listings { get; set; }

        public DbSet<UserProfile> Users { get; set; }

        public DbSet<ListingIcon> ListingIcons { get; set; }

        public DbSet<ListingCategory> ListingCategories { get; set; }

        public DbSet<ResetPasswordLog> ResetPasswordLog { get; set; }
    }
}