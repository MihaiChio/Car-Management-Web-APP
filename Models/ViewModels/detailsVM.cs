using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC.Models.ViewModels
{
    public class detailsVM
    {
        public detailsVM()
        {
            product = new Product();
        }

        public Product product { get;set; }
        public bool isInCart { get; set; }
    }
}
