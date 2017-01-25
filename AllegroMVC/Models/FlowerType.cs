using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllegroMVC.Models
{
    public class FlowerType
    {
        public int FlowerTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IconFileName { get; set; }

        public virtual ICollection<Flower> Flowers { get; set; }

        public int AllegroCategoryId { get; set; }
    }
}