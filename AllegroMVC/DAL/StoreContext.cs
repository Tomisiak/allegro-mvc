using AllegroMVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AllegroMVC.DAL
{
    public class StoreContext : IdentityDbContext<ApplicationUser>
    {
        public StoreContext() : base("StoreContext")
        {

        }

        static StoreContext()
        {
            Database.SetInitializer<StoreContext>(new StoreInitializer());
        }

        public static StoreContext Create()
        {
            return new StoreContext();
        }

        public DbSet<Flower> Flowers { get; set; }

        public DbSet<FlowerType> FlowerTypes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}