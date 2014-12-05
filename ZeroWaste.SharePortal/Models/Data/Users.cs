using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZeroWaste.SharePortal.Models.Data
{
    [Table("UserProfile")]
    public class UserProfile
    {
        private ICollection<Listing> _lisings;

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string OrganisationOrGroup { get; set; }
        public string GroupAddress { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public string AboutGroup { get; set; }
        //public Accout Accout { get; set; }

        
        public virtual ICollection<Listing> Listings
        {
            get { return _lisings ?? (_lisings = new Collection<Listing>()); }
            protected set { _lisings = value; }
        }
    
    }

    /// <summary>
    /// Indicates the state.
    /// </summary>
    /// <remarks>
    /// Do not change the order as the databas stores the INT value of the enum, and old
    /// records will then have the state value changed if the order changes when doing a linq
    /// query for where(l => l.State == State.SA)
    /// </remarks>
    public enum State
    {
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