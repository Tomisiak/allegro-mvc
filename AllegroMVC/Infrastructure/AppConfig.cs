using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AllegroMVC.Infrastructure
{
    public class AppConfig
    {
        private static string _flowerTypeIconsFolderRelative = ConfigurationManager.AppSettings["FlowerTypeIconsFolder"];

        public static string FlowerTypeIconsFolderRelative
        {
            get
            {
                return _flowerTypeIconsFolderRelative;
            }
        }

        private static string _photosFolderRelative = ConfigurationManager.AppSettings["PhotosFolder"];

        public static string PhotosFolderRelative
        {
            get
            {
                return _photosFolderRelative;
            }
        }

        private static string _allegroUser = ConfigurationManager.AppSettings["AllegroUser"];

        public static string AllegroUser
        {
            get
            {
                return _allegroUser;
            }
        }

        private static string _allegroWebApiKey = ConfigurationManager.AppSettings["AllegroWebApiKey"];

        public static string AllegroWebApiKey
        {
            get
            {
                return _allegroWebApiKey;
            }
        }

        private static string _allegroPasswordHash = ConfigurationManager.AppSettings["AllegroPasswordHash"];

        public static string AllegroPasswordHash
        {
            get
            {
                return _allegroPasswordHash;
            }
        }
    }
}