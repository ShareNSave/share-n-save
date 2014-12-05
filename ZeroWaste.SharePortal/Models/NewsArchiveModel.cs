using System;
using System.Collections.Generic;
using NHibernate.Classic;

namespace ZeroWaste.SharePortal.Models
{
    public class NewsArchiveModel
    {
        public IEnumerable<DateTime> PublicationDates { get; set; }
    }
}