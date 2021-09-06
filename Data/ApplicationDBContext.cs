using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1MVC.Models;

namespace WebApplication1MVC.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        //pushing to database

        public DbSet<Models.Category> Category { get; set; }
        public DbSet<Models.ApplicationType> ApplicationTypes { get; set; }
        public DbSet<Models.Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        //what do we want to create.
    }
}
