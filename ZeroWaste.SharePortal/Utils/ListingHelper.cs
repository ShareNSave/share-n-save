using System;
using System.Text;
using ZeroWaste.SharePortal.Extensions;
using ZeroWaste.SharePortal.Models.Data;

namespace ZeroWaste.SharePortal.Utils
{
    public static class ListingHelper
    {
        public const int characterCutOff = 200;

        public static string GetImageUrl(Listing listing)
        {
            if (string.IsNullOrWhiteSpace(listing.ListingImageLink))
            {
                var listingIcon = listing.ListingIcon.Name;

                return string.Format("/Content/images/defaultpics/{0}_image.jpg", listingIcon);
            }
            else
            {
                string filePath = System.Configuration.ConfigurationManager.AppSettings["ListingImageFilePath"];
                filePath = filePath + listing.ListingImageLink;

                return filePath;
            }
        }

        public static string CreatePopupHtmlContent(Listing item)
        {
            string category;
            int catId;
            return CreatePopupHtmlContent(item, out category, out catId);
        }

        public static string CreatePopupHtmlContent(Listing item, out string category, out int catId, bool includeBoxPoint = true)
        {
            category = string.Empty;
            catId = 0;
            if (item.ListingIcon.Category != null)
            {
                catId = item.ListingIcon.Category.Id;
                switch (catId)
                {
                    case 1: category = "share"; break;
                    case 2: category = "together"; break;
                    case 3: category = "borrow"; break;
                    default: category = string.Empty; break;
                }
            }

            var listingMessage = new StringBuilder();


            string _aboutGroup = item.AboutGroup;
            //// restrict length of About Group copy           
            //if (_aboutGroup.Length > characterCutOff)
            //{ 
            //    int _cutOffPos = _aboutGroup.LastIndexOf('.', characterCutOff);
            //    _aboutGroup = _aboutGroup.Substring(0, _cutOffPos >= 0 ? _cutOffPos : characterCutOff);
            //    //_aboutGroup += "...";
            //}

            listingMessage.Append("<ul id=\"popover-tabs\" class=\"nav nav-tabs\"><li class=\"active\"><a data-toggle=\"tab\" class=\"popover-tab\" href=\"#profile\">Profile</a></li><li><a data-toggle=\"tab\" class=\"popover-tab\" href=\"#connect\">Connect</a></li></ul>");
            listingMessage.Append("<div class=\"tab-content\"><div class=\"tab-pane active\" id=\"profile\">");
            listingMessage.AppendFormat("<p>{0}</p></div><div class=\"tab-pane\" id=\"connect\">", _aboutGroup);
            if (!string.IsNullOrWhiteSpace(item.ListingMessage))
            {
                //listingMessage.AppendFormat("<h3>Times/Access hours:</h3><p>{0}</p>", item.ListingMessage);
                listingMessage.AppendFormat("<p>{0}</p>", item.ListingMessage);
            }
            if (!string.IsNullOrWhiteSpace(item.Phone))
            {
                listingMessage.AppendFormat("<h4>Phone:<br/><span>{0}</span></h4>", item.Phone);
            }
            if (!string.IsNullOrWhiteSpace(item.Email))
            {
                listingMessage.AppendFormat("<h4>Email:<br/><a href='mailto:{0}'>{0}</a></h4>", item.Email);
            }
            if (!string.IsNullOrWhiteSpace(item.MapAddress))
            {
                listingMessage.AppendFormat("<h4>Address:<br/><span>{0}</span></h4>", item.MapAddress);
            }

            if (!string.IsNullOrWhiteSpace(item.WebLink))
            {
                string webLink = item.WebLink.ReplaceQuoteToEmpty();
                if (!webLink.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    webLink = "http://" + webLink;
                }

                listingMessage.AppendFormat("<div class='connectLink'><a href='{0}' target='_blank'>Connect to us</a></div>",
                    webLink);
            }
            listingMessage.Append("</div></div>");
            var listingImageLink = GetImageUrl(item);
            string content = string.Format(
                "{5}<div class=\"{4}Popover\"><h2>{0}</h2><img src='{1}' alt='{2}' width='200' height='133' />{3}</div>",
                item.Name, listingImageLink, item.Name.Trim(), listingMessage, category,
                includeBoxPoint ? "<div class=\"boxPoint\"></div>" : string.Empty);

            return content;
        }
    }
}