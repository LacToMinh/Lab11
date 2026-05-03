using ASC.Web.Data;
using ASC.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASC.DataAccess.Interfaces;
using ASC.DataAccess;

namespace ASC.Web.Services
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddCongfig(
        this IServiceCollection services,
        IConfiguration config)
    {
      // Add DbContext with connectionString
      var connectionString = config.GetConnectionString("DefaultConnection") ??
          throw new InvalidOperationException(
              "Connection string 'DefaultConnection' not found.");

      services.AddDbContext<ApplicationDbContext>(
          options => options.UseSqlServer(connectionString));

      // Add Options and get data from appsettings.json with "AppSettings"
      services.AddOptions(); // IOption
      services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));

      return services;
    }

    public static IServiceCollection AddMyDependencyGroup(
        this IServiceCollection services)
    {
      // Add ApplicationDbContext
      services.AddScoped<DbContext, ApplicationDbContext>();

      // Add Identity
      services.AddIdentity<IdentityUser, IdentityRole>(options =>
      {
        options.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders();

      // Add services
      services.AddTransient<IEmailSender, AuthMessageSender>();
      services.AddTransient<ISmsSender, AuthMessageSender>();
      services.AddSingleton<IIdentitySeed, IdentitySeed>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();

      // Add RazorPages, MVC
      services.AddRazorPages();
      services.AddDatabaseDeveloperPageExceptionFilter();
      services.AddControllersWithViews();

      // Add Cache, Session
      services.AddSession();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddDistributedMemoryCache();
      services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();

      return services;
    }
  }
}