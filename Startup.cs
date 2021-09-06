using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //config the options to use the sql server that was created earlier in appSettings.
            services.AddDbContext<Data.ApplicationDBContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"))); // looks in the app settings and search for the specified key("DefaultConnection")
            services.AddIdentity<Microsoft.AspNetCore.Identity.IdentityUser,Microsoft.AspNetCore.Identity.IdentityRole>()
                //automatically configure the role manager ^.
                .AddDefaultTokenProviders().AddDefaultUI() //default UI is for identity pages and a few more things.  
                //providing tokens when user forgets password.
                .AddEntityFrameworkStores<Data.ApplicationDBContext>(); //creates entity table within the specified database.
            services.AddHttpContextAccessor();
            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
             /// pipeline
            app.UseRouting();

            //after introducing identity, "UseAuthentication" should be used instead of Authorization. 
            //Authentication should be above authorization.
            //migration added to the database for the new authentication tables.
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession(); //.net core only supports integers and strings. Does not support objects or list by default. 
            app.UseEndpoints(endpoints =>
            {
                //add endpoints for razor pages
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
                //default route if nothing has been loaded, controller home and action index.
            });
        }
    }
}
