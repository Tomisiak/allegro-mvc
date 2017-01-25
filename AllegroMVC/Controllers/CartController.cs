using AllegroMVC.App_Start;
using AllegroMVC.DAL;
using AllegroMVC.Infrastructure;
using AllegroMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using AllegroMVC.Models;
using System.Threading.Tasks;

namespace AllegroMVC.Controllers
{
    public class CartController : Controller
    {
        private ShoppingCartManager shoppingCartManager;
        
        private ISessionManager sessionManger { get; set; }

        private StoreContext db = new StoreContext();

        public CartController()
        {
            sessionManger = new SessionManager();
            shoppingCartManager = new ShoppingCartManager(sessionManger, db);
        } 

        public ActionResult Index()
        {
            var cartItems = shoppingCartManager.GetCart();
            var cartTotalPrice = shoppingCartManager.GetCartTotalPrice();

            CartViewModel cartVM = new CartViewModel()
            {
                CartItems = cartItems,
                TotalPrice = cartTotalPrice
            };

            return View(cartVM);
        }

        public ActionResult AddToCart(int id)
        {
            shoppingCartManager.AddToCart(id);

            return RedirectToAction("Index");
        }

        public int GetCartItemsCount()
        {
            return shoppingCartManager.GetCartItemsCount();
        }

        public ActionResult RemoveFromCart(int flowerId)
        {
            int itemCount = shoppingCartManager.RemoveFromCart(flowerId);
            int cartItemsCount = shoppingCartManager.GetCartItemsCount();
            decimal cartTotal = shoppingCartManager.GetCartTotalPrice();

            // Return JSON to process it in JavaScript
            var result = new CartRemoveViewModel
            {
                RemoveItemId = flowerId,
                RemovedItemCount = itemCount,
                CartTotal = cartTotal,
                CartItemsCount = cartItemsCount
            };

            return Json(result);
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<ActionResult> Checkout()
        {
            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId()); // get logged in user info

                var order = new Order
                {
                    FirstName = user.UserData.FirstName,
                    LastName = user.UserData.LastName,
                    Address = user.UserData.Address,
                    CodeAndCity = user.UserData.CodeAndCity,
                    Email = user.UserData.Email,
                    PhoneNumber = user.UserData.PhoneNumber
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Cart") });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(Order orderDetails)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var newOrder = shoppingCartManager.CreateOrder(orderDetails, userId);

                var user = await UserManager.FindByIdAsync(userId);
                TryUpdateModel(user.UserData);
                await UserManager.UpdateAsync(user);

                shoppingCartManager.EmptyCart();

                return RedirectToAction("OrderConfirmation");
            } else
            {
                return View(orderDetails);
            }
        }

        public ActionResult OrderConfirmation()
        {
            return View();
        }
    }
}