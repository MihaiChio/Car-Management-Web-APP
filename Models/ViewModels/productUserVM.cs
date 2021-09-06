using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC.Models.ViewModels
{
    public class productUserVM
    {

        public productUserVM()
        {
            productList = new List<Product>();
        }

        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<Product> productList { get; set; }
    }
}
