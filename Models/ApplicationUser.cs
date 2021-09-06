using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string fullName { get; set; }
    }
}
