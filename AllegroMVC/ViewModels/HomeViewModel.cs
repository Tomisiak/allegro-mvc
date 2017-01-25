using AllegroMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllegroMVC.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Flower> Bestsellers { get; set; }

        public IEnumerable<Flower> NewArrivals { get; set; }

        public IEnumerable<FlowerType> FlowerTypes { get; set; }
    }
}