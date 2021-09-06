using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1MVC.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebApplication1MVC.Controllers
{
    [Authorize]//specifies that authorization is required.
    public class CartsController : Controller
    {
        private readonly Data.ApplicationDBContext _db;

        [BindProperty]
        public Models.ViewModels.productUserVM  productUserVM {get; set; }

        public CartsController(Data.ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Models.shoppingCart> shoppingCartList = new List<Models.shoppingCart>();
            //retrieving the items within the session. ( cart)
            if (HttpContext.Session.Get<IEnumerable<Models.shoppingCart>>(WebConstants.sessionCart) != null
               && HttpContext.Session.Get<IEnumerable<Models.shoppingCart>>(WebConstants.sessionCart).Count() > 0)
            {
                //populating the list
                shoppingCartList = HttpContext.Session.Get<List<Models.shoppingCart>>(WebConstants.sessionCart);
            }

            List<int> productInCart = shoppingCartList.Select(i => i.productId).ToList();
            // list of all the product IDs within the card, this is used so later on we can retrieve the objects based on their ID.
            IEnumerable<Models.Product> productList = _db.Product.Where(u => productInCart.Contains(u.id));
            //will retrieve any product with the ID that matches the "productInCart" ID.

            return View(productList);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {

            return RedirectToAction(nameof(Summary));

        }

        public IActionResult Summary()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //"claim" will only be populated if a user is logged in.
            //another method to see if someone's logged in.
            //var userID = User.FindFirstValue(ClaimTypes.Name);

            List<Models.shoppingCart> shoppingCartList = new List<Models.shoppingCart>();
            //retrieving the items within the session. ( cart)
            if (HttpContext.Session.Get<IEnumerable<Models.shoppingCart>>(WebConstants.sessionCart) != null
               && HttpContext.Session.Get<IEnumerable<Models.shoppingCart>>(WebConstants.sessionCart).Count() > 0)
            {
                //populating the list
                shoppingCartList = HttpContext.Session.Get<List<Models.shoppingCart>>(WebConstants.sessionCart);
            }

            List<int> productInCart = shoppingCartList.Select(i => i.productId).ToList();
            // list of all the product IDs within the card, this is used so later on we can retrieve the objects based on their ID.
            IEnumerable<Models.Product> productList = _db.Product.Where(u => productInCart.Contains(u.id));
            //will retrieve any product with the ID that matches the "productInCart" ID.


            //loading the session again.

            productUserVM = new Models.ViewModels.productUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value)
            };

            return View(productUserVM);

        }


        public IActionResult Delete(int id)
        {
            List<Models.shoppingCart> shoppingCartList = new List<Models.shoppingCart>();
            //retrieving the items within the session. ( cart)
            if (HttpContext.Session.Get<IEnumerable<Models.shoppingCart>>(WebConstants.sessionCart) != null
               && HttpContext.Session.Get<IEnumerable<Models.shoppingCart>>(WebConstants.sessionCart).Count() > 0)
            {
                //populating the list
                shoppingCartList = HttpContext.Session.Get<List<Models.shoppingCart>>(WebConstants.sessionCart);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.productId == id));
            //finds that object and removes it from the list.

            HttpContext.Session.Set(WebConstants.sessionCart, shoppingCartList);
            //setting the session again since we removed something.
            return RedirectToAction(nameof(Index));
        }
    }
}
