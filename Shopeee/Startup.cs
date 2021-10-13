using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopeee.Data;
using Shopeee.Models;
using Microsoft.AspNetCore.Identity;
using Shopeee.Areas.Identity;

namespace Shopeee
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
            services.AddControllersWithViews();

            services.AddDbContext<ShopeeeContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ShopeeeContext")));

            //services.AddDefaultIdentity<ApplicationUser>(options => options.Password.RequireNonAlphanumeric = false)
            //     .AddEntityFrameworkStores<ShopeeeContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddDefaultUI()
                .AddEntityFrameworkStores<ShopeeeContext>()
                .AddDefaultTokenProviders();
            services.AddRazorPages();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("readpolicy",
                    builder => builder.RequireRole("Administrator", "Branch Manager", "Client"));
                options.AddPolicy("writepolicy",
                    builder => builder.RequireRole("Administrator", "Branch Manager"));
            });
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
