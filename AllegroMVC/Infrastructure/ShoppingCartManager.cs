using AllegroMVC.DAL;
using AllegroMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllegroMVC.Infrastructure
{
    public class ShoppingCartManager
    {
        private StoreContext db;

        private ISessionManager session;

        public const string CART_SESSION_KEY = "CartData";

        public ShoppingCartManager(ISessionManager session, StoreContext db)
        {
            this.session = session;
            this.db = db;
        }

        public void AddToCart(int flowerId)
        {
            var cart = GetCart();

            var cartItem = cart.Find(c => c.Flower.FlowerId == flowerId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                // Find flower and add it to cart
                var flowerToAdd = db.Flowers.Where(f => f.FlowerId == flowerId).SingleOrDefault();
                if (flowerToAdd != null)
                {
                    var newCartItem = new CartItem()
                    {
                        Flower = flowerToAdd,
                        Quantity = 1,
                        TotalPrice = flowerToAdd.Price
                    };

                    cart.Add(newCartItem);
                }
            }

            session.Set(CART_SESSION_KEY, cart);
        }

        public List<CartItem> GetCart()
        {
            List<CartItem> cart;

            if (session.Get<List<CartItem>>(CART_SESSION_KEY) == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = session.Get<List<CartItem>>(CART_SESSION_KEY) as List<CartItem>;
            }

            return cart;
        }

        public int RemoveFromCart(int flowerId)
        {
            var cart = GetCart();

            var cartItem = cart.Find(c => c.Flower.FlowerId == flowerId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    return cartItem.Quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
            }

            // Return count of removed item currently inside cart
            return 0;
        }

        public decimal GetCartTotalPrice()
        {
            var cart = GetCart();

            return cart.Sum(c => (c.Quantity * c.Flower.Price));
        }

        public int GetCartItemsCount()
        {
            var cart = GetCart();
            int count = cart.Sum(c => c.Quantity);

            return count;
        }

        public Order CreateOrder(Order newOrder, string userId)
        {
            var cart = this.GetCart();

            newOrder.DateCreated = DateTime.Now;
            newOrder.UserId = userId;

            db.Orders.Add(newOrder);

            if (newOrder.OrderItems == null)
            {
                newOrder.OrderItems = new List<OrderItem>();
            }

            decimal cartTotal = 0;

            foreach (var cartItem in cart)
            {
                var newOrderItem = new OrderItem()
                {
                    FlowerId = cartItem.Flower.FlowerId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Flower.Price
                };

                cartTotal += (cartItem.Quantity * cartItem.Flower.Price);

                newOrder.OrderItems.Add(newOrderItem);
            }

            newOrder.TotalPrice = cartTotal;

            db.SaveChanges();

            return newOrder;
        }

        public void EmptyCart()
        {
            session.Set<List<CartItem>>(CART_SESSION_KEY, null);
        }
    }
}