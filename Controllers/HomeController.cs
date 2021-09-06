using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1MVC.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1MVC.Utility;

namespace WebApplication1MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Data.ApplicationDBContext _db;

        public HomeController(ILogger<HomeController> logger, Data.ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            Models.ViewModels.homeVM homeVM = new Models.ViewModels.homeVM()
            {
                products = _db.Product.Include(u => u.category).Include(u => u.applicationType), //eager loading.
                categories = _db.Category
            };//populating a new model.
            return View(homeVM);
        }

        public IActionResult details(int id)
        {
            List<shoppingCart> shoppingCarts = new List<shoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<shoppingCart>>(WebConstants.sessionCart) != null
                && HttpContext.Session.Get<IEnumerable<shoppingCart>>(WebConstants.sessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<shoppingCart>>(WebConstants.sessionCart);
            }


            Models.ViewModels.detailsVM DetailsVM = new Models.ViewModels.detailsVM()
            {
                product = _db.Product.Include(u => u.category).Include(u => u.applicationType).Where(u => u.id == id).FirstOrDefault(),
                isInCart = false
            };

            foreach(Models.shoppingCart item in shoppingCarts)
            {
                if(item.productId == id)
                {
                    DetailsVM.isInCart = true;
                }
                else
                {
                    DetailsVM.isInCart = false;
                }
            }

            return View(DetailsVM);
        }

        //post details
        [HttpPost,ActionName("Details")]
        public IActionResult detailsPost(int id)
        {
            List<shoppingCart> shoppingCarts = new List<shoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<shoppingCart>>(WebConstants.sessionCart) != null
                && HttpContext.Session.Get<IEnumerable<shoppingCart>>(WebConstants.sessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<shoppingCart>>(WebConstants.sessionCart);
            }

            shoppingCarts.Add(new shoppingCart { productId = id });
            HttpContext.Session.Set(WebConstants.sessionCart, shoppingCarts);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult removeFromCart(int id)
        {
            List<shoppingCart> shoppingCarts = new List<shoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<shoppingCart>>(WebConstants.sessionCart) != null
                && HttpContext.Session.Get<IEnumerable<shoppingCart>>(WebConstants.sessionCart).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<shoppingCart>>(WebConstants.sessionCart);
            }

            Models.shoppingCart itemToRemove = shoppingCarts.SingleOrDefault(r => r.productId == id);
            if(itemToRemove != null)
            {
                shoppingCarts.Remove(itemToRemove); // remove the item from your garage.
            }



            HttpContext.Session.Set(WebConstants.sessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Exercises()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
