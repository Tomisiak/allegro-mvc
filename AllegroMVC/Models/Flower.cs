using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllegroMVC.Models
{
    public class Flower
    {
        public int FlowerId { get; set; }
        
        public int FlowerTypeId { get; set; }

        public string FlowerName { get; set; }

        public DateTime DateAdded { get; set; }

        public string ImageFileName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsBestseller { get; set; }

        public bool IsHidden { get; set; }

        public virtual FlowerType FlowerType { get; set; }

        public long AllegroAuctionId { get; set; }
    }
}