using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication8.Areas.Identity.Data;
using WebApplication8.Data;

[assembly: HostingStartup(typeof(WebApplication8.Areas.Identity.IdentityHostingStartup))]
namespace WebApplication8.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            /* builder.ConfigureServices((context, services) => {
                 services.AddDbContext<WebAppContext>(options =>
                     options.UseSqlServer(
                         context.Configuration.GetConnectionString("WebApplication8ContextConnection")));*/

            /*    services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<WebAppContext>();
            });*/            
        }
    }
}