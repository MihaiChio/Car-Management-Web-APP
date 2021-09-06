using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC.Models.ViewModels
{
    public class homeVM
    {
        public IEnumerable<Product> products { get; set; }

        public IEnumerable<Category> categories { get; set; }
    }
}
