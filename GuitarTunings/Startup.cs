using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GuitarTunings.Models;
using Microsoft.AspNetCore.Identity;

namespace GuitarTunings
{
  public class Startup
  {
    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json");
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      services.AddEntityFrameworkSqlServer()
        .AddDbContext<GuitarTuningsContext>(options => options
        .UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

      services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<GuitarTuningsContext>()
                .AddDefaultTokenProviders();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseDeveloperExceptionPage();

      app.UseAuthentication(); 

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(routes =>
      {
        routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });

      app.UseStaticFiles();

      app.Run(async (context) =>
      {
        context.Response.Redirect("/NotFound");
        // await context.Response.WriteAsync("Error, URL path does not exist");
      });
    }
  }
}