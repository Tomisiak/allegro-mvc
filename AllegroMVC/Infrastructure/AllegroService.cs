using AllegroMVC.AllegroSandbox;
using AllegroMVC.DAL;
using AllegroMVC.Models;
using AllegroMVC.ViewModels;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AllegroMVC.Infrastructure
{
    public class AllegroService
    {
        StoreContext db = new StoreContext();

        const string SHIPMENT_ADDITIONAL_INFO = "Przedmioty staramy się wysyłać do 2 dni od otrzymania wpłaty. Maksymalny czas na wysłanie towaru wynosi 7 dni od otrzymania wpłaty.";

        public static string HashPassword(string password)
        {
            SHA256Managed shaEncoder = new SHA256Managed();
            UTF8Encoding encoding = new UTF8Encoding();

            byte[] toencode = Encoding.UTF8.GetBytes(password);
            string passwordHash = Convert.ToBase64String(shaEncoder.ComputeHash(toencode));

            return passwordHash;
        }

        public static string Login(AllegroUserData allegroUserData)
        {
            servicePortClient allegroClient = new servicePortClient();

            long userid, serverTime;

            long verkey;
            allegroClient.doQuerySysStatus(1, 1, allegroUserData.AllegroWebApiKey, out verkey);

            string sessionid = allegroClient.doLoginEnc(allegroUserData.AllegroLogin, allegroUserData.AllegroPassword, 1, allegroUserData.AllegroWebApiKey, verkey, out userid, out serverTime);

            return sessionid;
        }

        public long CreateAuction(int flowerId, AllegroUserData allegroUserData)
        {
            var flower = db.Flowers.Find(flowerId);

            servicePortClient allegroClient = new servicePortClient();

            var sessionid = Login(allegroUserData);

            string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(AppConfig.PhotosFolderRelative), flower.ImageFileName);
            byte[] imageBytes = File.ReadAllBytes(filePath);

            string templatePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/AllegroTemplates"), "AllegroBasic.cshtml");
            ITemplateServiceConfiguration configuration = new TemplateServiceConfiguration();
            IRazorEngineService service = RazorEngineService.Create(configuration);

            string description = "";
            string template = File.ReadAllText(templatePath);

            var model = new AllegroViewModel { Flower = flower };
            description = service.RunCompile(template, "templateKey", null, model);

            List<FieldsValue> sellFieldsList = new List<FieldsValue>();
            sellFieldsList.AddRange(new FieldsValue[]
            {
                new FieldsValue() { fid = 1, fvalueString = flower.FlowerName },  // nazwa
                new FieldsValue() { fid = 2, fvalueInt = flower.FlowerType.AllegroCategoryId, fvalueIntSpecified = true }, // kategoria
                new FieldsValue() { fid = 4, fvalueInt = 99, fvalueIntSpecified = true }, // czas trwania (do wyczerpania)
                new FieldsValue() { fid = 5, fvalueInt = 1, fvalueIntSpecified = true }, // liczba sztuk
                new FieldsValue() { fid = 9, fvalueInt = 1, fvalueIntSpecified = true }, // kraj (Polska)
                new FieldsValue() { fid = 10, fvalueInt = 12, fvalueIntSpecified = true }, // województwo (śląskie)
                new FieldsValue() { fid = 11, fvalueString = "Gliwice" }, // miasto
                new FieldsValue() { fid = 24, fvalueString = description }, // opis
                new FieldsValue() { fid = 8, fvalueFloat = (float)flower.Price, fvalueFloatSpecified = true }, // cena "Kup Teraz!"
                new FieldsValue() { fid = 12, fvalueInt = 1, fvalueIntSpecified = true }, // koszt przesyłki pokrywa (kupujący)
                new FieldsValue() { fid = 14, fvalueInt = 33, fvalueIntSpecified = true }, // formy płatności
                new FieldsValue() { fid = 32, fvalueString = "44-100" }, // kod pocztowy
                new FieldsValue() { fid = 33, fvalueString = "69 1160 2202 0000 0002 5948 9791" }, // numer konta bankowego
                new FieldsValue() { fid = 35, fvalueInt = 0, fvalueIntSpecified = true },
                // DOSTAWA
                new FieldsValue() { fid = 44, fvalueFloat = (float)12.5, fvalueFloatSpecified = true }, // przesyłka kurierska (pierwsza sztuka)
                new FieldsValue() { fid = 144, fvalueFloat = (float)0, fvalueFloatSpecified = true }, // przesyłka kurierska (kolejna sztuka)
                new FieldsValue() { fid = 244, fvalueInt = 999, fvalueIntSpecified = true }, // przesyłka kurierska (ilość w paczce)

                new FieldsValue() { fid = 52, fvalueFloat = (float)9.99, fvalueFloatSpecified = true }, // Odbiór w punkcie - E-PRZESYŁKA / Paczka48 Odbiór w Punkcie (pierwsza sztuka)
                new FieldsValue() { fid = 152, fvalueFloat = (float)0, fvalueFloatSpecified = true }, // Odbiór w punkcie - E-PRZESYŁKA / Paczka48 Odbiór w Punkcie (kolejna sztuka)
                new FieldsValue() { fid = 252, fvalueInt = 999, fvalueIntSpecified = true }, // Odbiór w punkcie - E-PRZESYŁKA / Paczka48 Odbiór w Punkcie (ilość w paczce)

                new FieldsValue() { fid = 45, fvalueFloat = (float)15, fvalueFloatSpecified = true }, // przesyłka kurierska pobraniowa (pierwsza sztuka)
                new FieldsValue() { fid = 145, fvalueFloat = (float)0, fvalueFloatSpecified = true }, // przesyłka kurierska pobraniowa (kolejna sztuka)
                new FieldsValue() { fid = 245, fvalueInt = 999, fvalueIntSpecified = true }, // przesyłka kurierska pobraniowa (ilość w paczce)
                // END DOSTAWA
                //new FieldsValue() { fid = 340, fvalueInt = 24, fvalueIntSpecified = true }, // wysyłka w ciągu
                new FieldsValue() { fid = 15, fvalueInt = 2, fvalueIntSpecified = true }, // opcje dodatkowe (miniaturka)
                new FieldsValue() { fid = 16, fvalueImage = imageBytes }, // zdjęcie 1 z 8
                new FieldsValue() { fid = 20634, fvalueInt = 1, fvalueIntSpecified = true }, // stan (nowy)
                new FieldsValue() { fid = 27, fvalueString = SHIPMENT_ADDITIONAL_INFO } // dodatkowe informacje o przesyłce i płatności
            });

            int isallegrostandard;
            string iteminfo;

            long auctionnum = allegroClient.doNewAuctionExt(sessionid, sellFieldsList.ToArray(), 0, 0, null, null, null, out iteminfo, out isallegrostandard);

            flower.AllegroAuctionId = auctionnum;
            db.SaveChanges();

            return auctionnum;
        }
    }
}