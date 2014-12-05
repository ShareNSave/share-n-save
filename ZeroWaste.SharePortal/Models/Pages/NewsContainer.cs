using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using N2;
using N2.Collections;
using N2.Definitions;
using N2.Integrity;

namespace ZeroWaste.SharePortal.Models
{
    [PageDefinition("News Container",
        Description = "A list of news. News items can be added to this page.",
        SortOrder = 150,
        IconClass = "n2-icon-list blue")]
    [RestrictParents(typeof(IStructuralPage))]
    [SortChildren(SortBy.PublishedDescending)]
    [GroupChildren(GroupChildrenMode.PublishedYear)]
    [AvailableZone("To the left on this and child pages.", "RecursiveLeft")]
    [AvailableZone("To the left on this.", "Left")]
    [AvailableZone("To the right on this and child pages.", "RecursiveRight")]
    [AvailableZone("To the right on this.", "Right")]
    public class NewsContainer : ContentPageBase
    {
        public IEnumerable<News> NewsItems
        {
            get { return GetChildren(new TypeFilter(typeof(News))).OfType<News>(); }
        }
    }
}