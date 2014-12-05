using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using ZeroWaste.SharePortal.Models;
using ZeroWaste.SharePortal.Models.Parts;
using N2.Web;
using N2.Web.Mvc;

namespace ZeroWaste.SharePortal.Controllers
{
    [Controls(typeof(NewsArchivePart))]
    public class NewsArchiveController : ContentController<NewsArchivePart>
    {
        //
        // GET: /NewsArchivePart/
        public override ActionResult Index()
        {
            string cacheKey = string.Format("{0}-news-archives", CurrentItem.ID);
            string cacheUpdatingKey = string.Format("{0}-news-archives-updating", CurrentItem.ID);
            string cacheTimeKey = string.Format("{0}-news-archives-time", CurrentItem.ID);

            NewsArchiveModel model;
            if (HttpContext.Cache[cacheTimeKey] == null)
            {
                if (HttpContext.Cache[cacheUpdatingKey] != null && HttpContext.Cache[cacheKey] != null)
                {
                    model = CreateModel(HttpContext.Cache[cacheKey] as IEnumerable<DateTime>);
                }
                else
                {
                    HttpContext.Cache.Insert(cacheUpdatingKey, true, null, DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration);

                    model = CreateModel(NewsArchives);
                    HttpContext.Cache.Insert(cacheKey, model.PublicationDates, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration);

                    HttpContext.Cache.Insert(cacheTimeKey, true, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                    HttpContext.Cache.Remove(cacheUpdatingKey);
                }
            }
            else
            {
                model = CreateModel(HttpContext.Cache[cacheKey] as IEnumerable<DateTime>);
            }

            return PartialView("PartTemplates/NewsArchive", model);
        }

        private NewsArchiveModel CreateModel(IEnumerable<DateTime> dateTimes)
        {
            return new NewsArchiveModel
            {
                PublicationDates = dateTimes
            };
        }

        public IEnumerable<DateTime> NewsArchives
        {
            get
            {
                IEnumerable<News> news = N2.Find.Items.Where.Type.Eq(typeof(News))
                    .Filters(new N2.Collections.PublishedFilter())
                    .OrderBy.Detail("Published").Desc
                    .Select<News>();

                return (from item in news
                        select new DateTime(item.Published.Value.Year, item.Published.Value.Month, 1)
                        ).Distinct().OrderByDescending(x => x);
            }
        }
    }
}
