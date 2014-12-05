using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroWaste.SharePortal.Models
{
    public class NewsContainerModel
    {
        public string Title { get; set; }
        public NewsContainer Container { get; set; }
        public IList<News> News { get; set; }

        public bool IsLast { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public string Tag { get; set; }
    }
}