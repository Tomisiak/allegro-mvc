using AllegroMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace AllegroMVC.DAL
{
    public class StoreInitializer : DropCreateDatabaseAlways<StoreContext>
    {
        protected override void Seed(StoreContext context)
        {
            SeedStoreData(context);
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private void SeedStoreData(StoreContext context)
        {
            var flowerTypes = new List<FlowerType>
            {
                new FlowerType() { FlowerTypeId = 1, Name = "Słoneczniki", IconFileName = "sunflower.png" , AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 2, Name = "Piwonie", IconFileName = "peony.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 3, Name = "Gladiole", IconFileName = "gladiola.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 4, Name = "Chryzantemy", IconFileName = "chrysanthemum.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 5, Name = "Dalie", IconFileName = "dahlia.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 6, Name = "Astry", IconFileName = "aster.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 7, Name = "Amarylisy", IconFileName = "amarylis.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 8, Name = "Hiacynty", IconFileName = "hyacinth.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 9, Name = "Tulipany", IconFileName = "tulip.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 10, Name = "Róże", IconFileName = "rose.png", AllegroCategoryId = 16008 },
                new FlowerType() { FlowerTypeId = 12, Name = "Promocje", IconFileName = "present.png", AllegroCategoryId = 16008 }
            };

            flowerTypes.ForEach(ft => context.FlowerTypes.Add(ft));
            context.SaveChanges();

            var flowers = new List<Flower>
            {
                new Flower() { FlowerId = 1, FlowerName = "Słonecznik1", Price = 12, ImageFileName = "1.jpg", IsBestseller = true, DateAdded = new DateTime(2014, 02, 1), FlowerTypeId = 1 },
                new Flower() { FlowerId = 2, FlowerName = "Słonecznik2", Price = 5, ImageFileName = "2.jpg", IsBestseller = true, DateAdded = new DateTime(2013, 08, 15), FlowerTypeId = 1 },
                new Flower() { FlowerId = 3, FlowerName = "Chryzantema1", Price = 6, ImageFileName = "3.jpg", IsBestseller = true, DateAdded = new DateTime(2014, 01, 5), FlowerTypeId = 4 },
                new Flower() { FlowerId = 4, FlowerName = "Chryzantema2", Price = 8, ImageFileName = "4.jpg", IsBestseller = true, DateAdded = new DateTime(2014, 03, 11), FlowerTypeId = 4 },
                new Flower() { FlowerId = 5, FlowerName = "Chryzantema3", Price = 11, ImageFileName = "5.jpg", IsBestseller = false, DateAdded = new DateTime(2014, 04, 2), FlowerTypeId = 4 },
                new Flower() { FlowerId = 6, FlowerName = "Chryzantema4", Price = 17, ImageFileName = "6.jpg", IsBestseller = false, DateAdded = new DateTime(2014, 04, 2), FlowerTypeId = 4 },
                new Flower() { FlowerId = 7, FlowerName = "Chryzantema5", Price = 6, ImageFileName = "7.jpg", IsBestseller = false, DateAdded = new DateTime(2014, 04, 2), FlowerTypeId = 4 },
                new Flower() { FlowerId = 8, FlowerName = "Chryzantema6", Price = 8, ImageFileName = "8.jpg", IsBestseller = false, DateAdded = new DateTime(2014, 04, 2), FlowerTypeId = 4 },
                new Flower() { FlowerId = 9, FlowerName = "Róża1", Price = 10, ImageFileName = "9.jpg", IsBestseller = false, DateAdded = new DateTime(2014, 04, 2), FlowerTypeId = 10 },
                new Flower() { FlowerId = 10, FlowerName = "Słonecznik3", Price = 5, ImageFileName = "10.jpg", IsBestseller = true, DateAdded = new DateTime(2013, 08, 15), FlowerTypeId = 1 }
            };

            flowers.ForEach(f => context.Flowers.Add(f));
            context.SaveChanges();
        }

        public static void InitializeIdentityForEF(StoreContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string name = "admin@admin.com";
            const string password = "Qwerty1@";
            const string roleName = "Admin";

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, UserData = new UserData(), AllegroUserData = new AllegroUserData() };
                //user = new ApplicationUser { UserName = name, Email = name, UserData = new UserData() };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleResult = roleManager.Create(role);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}