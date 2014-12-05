using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using N2.Engine;
using N2.Web.Drawing;

namespace ZeroWaste.SharePortal.Services
{
    [Service]
    [Service(typeof(IListingImageService))]
    public class ListingImageService : IListingImageService
    {
        public string SaveImage(HttpPostedFileBase file)
        {
            string imageUrl = null;
            if (file != null && !string.IsNullOrWhiteSpace(file.FileName))
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                // Separeate the uploaded images into category folders to reduce the number of files per folder. Windows doesnt like lots of files in a single folder.
                var now = DateTime.UtcNow;
                imageUrl = string.Format("/Content/Uploads/{0}/{1}/{2}/{3}", now.Year, now.Month, now.Day, fileName);

                // Map the relative url to a file path.
                var path = HttpContext.Current.Request.MapPath(imageUrl);
                var imageFile = new FileInfo(path);

                // Ensure the folder exists.
                if (!Directory.Exists(imageFile.Directory.FullName))
                    Directory.CreateDirectory(imageFile.Directory.FullName);

                // store the uploaded file on the file system
                file.SaveAs(imageFile.FullName);

                //resize the image
                try
                {
                    StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(imageUrl));
                    ImageResizeParameters resizeParameters = new ImageResizeParameters(this.ListingImageMaxWidth, this.ListingImageMaxHeight, ImageResizeMode.Fit);

                    int extIndex = imageUrl.LastIndexOf(".");
                    string filename = imageUrl.Substring(0, extIndex);
                    filename = filename + "_resized";
                    filename = filename + imageUrl.Substring(extIndex);

                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(filename), FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs);

                    ImageResizer imageResizer = new ImageResizer();

                    imageResizer.Resize(sr.BaseStream, resizeParameters, sw.BaseStream);

                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    sr.Close();
                    
                    if (imageFile.Exists)
                    {
                        try
                        {
                            imageFile.Delete();
                            imageFile = null;
                        }
                        catch { }
                    }

                    imageUrl = filename;
                }
                catch
                {

                }
            }

            return imageUrl;
        }


        public double ListingImageMaxWidth
        {
            get
            {
                double listingImageMaxWidth = 200;
                string widthStr = System.Configuration.ConfigurationManager.AppSettings["listingImageMaxWidth"];
                if (!string.IsNullOrWhiteSpace(widthStr))
                {
                    double.TryParse(widthStr, out listingImageMaxWidth);
                }
                return listingImageMaxWidth;
            }
        }

        public double ListingImageMaxHeight
        {
            get
            {
                double listingImageMaxHeight = 133;
                string heightStr = System.Configuration.ConfigurationManager.AppSettings["listingImageMaxHeight"];
                if (!string.IsNullOrWhiteSpace(heightStr))
                {
                    double.TryParse(heightStr, out listingImageMaxHeight);
                }
                return listingImageMaxHeight;
            }
        }
    }

    public interface IListingImageService
    {
        string SaveImage(HttpPostedFileBase file);
        double ListingImageMaxWidth { get; }
        double ListingImageMaxHeight { get; }
    }
}