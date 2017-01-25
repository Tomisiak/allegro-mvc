using AllegroMVC.DAL;
using AllegroMVC.Models;
using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllegroMVC.Infrastructure
{
    public class ProductDetailsDynamicNodeProvider : DynamicNodeProviderBase
    {
        private StoreContext db = new StoreContext(); 

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var returnValue = new List<DynamicNode>();

            foreach (Flower f in db.Flowers)
            {
                DynamicNode n = new DynamicNode();
                n.Title = f.FlowerName;
                n.Key = "Flower_" + f.FlowerId;
                n.ParentKey = "FlowerType_" + f.FlowerTypeId;
                n.RouteValues.Add("id", f.FlowerId);
                returnValue.Add(n);
            }

            return returnValue;
        }
    }
}