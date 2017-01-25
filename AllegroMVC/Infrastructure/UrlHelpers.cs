using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllegroMVC.Infrastructure
{
    public static class UrlHelpers
    {
        public static string FlowerTypeIconPath(this UrlHelper helper, string flowerTypeIconFilename)
        {
            var flowerTypeIconFolder = AppConfig.FlowerTypeIconsFolderRelative;
            var path = Path.Combine(flowerTypeIconFolder, flowerTypeIconFilename);
            var absolutePath = helper.Content(path);

            return absolutePath;
        }

        public static string FlowerPhotoPath(this UrlHelper helper, string flowerFilename)
        {
            var flowerPhotoFolder = AppConfig.PhotosFolderRelative;
            var path = Path.Combine(flowerPhotoFolder, flowerFilename);
            var absolutePath = helper.Content(path);

            return absolutePath;
        }
    }
}