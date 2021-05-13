using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTODOapp.Areas.Identity.Data;
using WebTODOapp.Data;

[assembly: HostingStartup(typeof(WebTODOapp.Areas.Identity.IdentityHostingStartup))]
namespace WebTODOapp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<OAuthContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("OAuthContextConnection")));

                services.AddDefaultIdentity<WebTODOappUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<OAuthContext>();
            });
        }
    }
}