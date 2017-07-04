using ICS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ICS.Utilities
{
    public class CustomMethods
    {
        public void ImageUploadValidation(ModelStateDictionary ModelState, HttpPostedFileBase file, string image)
        {
            if (file == null)
                ModelState.AddModelError(image, "Şəkil əlavə olunmayıb.");

            else if (file.ContentLength == 0)
                ModelState.AddModelError(image, "Faylin hecmi 0 dir.");

            else if (file.ContentLength > 10485760)
                ModelState.AddModelError(image, "Fayl 10 MB dan artiq ola bilməz!");

            else
            {
                var fileExt = Path.GetExtension(file.FileName).ToLower();
                if (!(fileExt.EndsWith(".jpg")
                || fileExt.EndsWith(".png")
                || fileExt.EndsWith(".gif")
                || fileExt.EndsWith(".jpeg")
                || fileExt.EndsWith(".bmp")
                || fileExt.EndsWith(".tif")
                || fileExt.EndsWith(".tiff"))
                   ) ModelState.AddModelError("about_Translate.About.image", "Siz şəkil əlavə etməmisiniz!");
            }
        }
        public void ImageUpload(HttpPostedFileBase file, string image)
        {
            byte[] productPicture = new byte[file.ContentLength];
            file.InputStream.Read(productPicture, 0, file.ContentLength);
            var img = new WebImage(file.InputStream);

            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/images/"), image);
            img.Save(path, "jpg");
        }
        public void ImageDelete(string image)
        {
            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/images/"), image);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists) file.Delete();
        }        
    }
}