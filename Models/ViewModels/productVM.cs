using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC.Models.ViewModels
{
    public class productVM
    {
       public Models.Product product { get; set; }
       public IEnumerable<SelectListItem> categorySelectList { get; set; }
       public IEnumerable<SelectListItem> applicationSelectList { get; set; }
        //A view model should be created when a Model object does not contain all the data a view needs.
    }
}
