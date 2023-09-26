using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OBYS.Core.Service;
using OBYS.Model.Context;
using OBYS.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OBYS.WebUI
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
            services.AddMvc();
            services.AddDbContext<OBYSContext>(options => options.UseSqlServer("Server=SA103HOCA\\SQLEXPRESS;Database=OBYS;User Id=sa;Password=1;"));

            // Dependecny Injection : Amacý baðlýklýklarý minimum seviyeye indirmek
            services.AddScoped(typeof(ICoreService<>), typeof(BaseService<>));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/Account/Login");
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); // Login yöntemini kullanacaðýz dedik
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {

                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "User",
                    pattern: "User/{Controller=Home}/{Action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{Controller=Home}/{Action=Index}/{id?}"
                );
            });
        }
    }
}
