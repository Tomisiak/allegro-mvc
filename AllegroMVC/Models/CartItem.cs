using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllegroMVC.Models
{
    public class CartItem
    {
        public Flower Flower { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}