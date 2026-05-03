using ASC.Web.Configuration;
using ASC.Web.Data;
using ASC.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCongfig(builder.Configuration)
    .AddMyDependencyGroup();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseMigrationsEndPoint();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Seed user data
using (var scope = app.Services.CreateScope())
{
  var storageSeed = scope.ServiceProvider
      .GetRequiredService<IIdentitySeed>();

  await storageSeed.Seed(
      scope.ServiceProvider.GetService<UserManager<IdentityUser>>(),
      scope.ServiceProvider.GetService<RoleManager<IdentityRole>>(),
      scope.ServiceProvider.GetService<IOptions<ApplicationSettings>>()
  );
}

//CreateNavigationCache
using (var scope = app.Services.CreateScope())
{
  var navigationCacheOperations = scope.ServiceProvider
      .GetRequiredService<INavigationCacheOperations>();
  await navigationCacheOperations.CreateNavigationCacheAsync();
}

app.Run();