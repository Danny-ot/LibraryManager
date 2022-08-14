using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Library.Models;
namespace Library
{
  public class Startup
  {
    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json"); //this line replaces .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set;}

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
       //New code
      services.AddEntityFrameworkMySql()
        .AddDbContext<LibraryContext>(options => options
        .UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));

      services.AddIdentity<LibraryUser, IdentityRole>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<LibraryContext>()
        .AddDefaultTokenProviders();

      services.Configure<IdentityOptions>(options => options.Password.RequireNonAlphanumeric = false);
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
        await context.Response.WriteAsync("Hello World!");
      });
    }
  }
}