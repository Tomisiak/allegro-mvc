using AllegroMVC.DAL;
using AllegroMVC.Models;
using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllegroMVC.Infrastructure
{
    public class ProductListDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext db = new StoreContext();

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();

            foreach (FlowerType ft in db.FlowerTypes)
            {
                DynamicNode n = new DynamicNode();
                n.Title = ft.Name;
                n.Key = "FlowerType_" + ft.FlowerTypeId;
                n.RouteValues.Add("flowerTypeName", ft.Name);
                returnValue.Add(n);
            }

            return returnValue;
        }
    }
}