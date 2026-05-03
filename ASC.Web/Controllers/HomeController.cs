using System.Diagnostics;
using ASC.Utilities;
using ASC.Web.Configuration;
using ASC.Web.Models;
using ASC.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASC.Web.Controllers
{
  public class HomeController : AnonymousController
  {
    private readonly ILogger<HomeController> _logger;
    private IOptions<ApplicationSettings> _settings;

    public HomeController(ILogger<HomeController> logger, IOptions<ApplicationSettings> settings)
    {
      _logger = logger;
      _settings = settings;
    }

    public IActionResult Index([FromServices] IEmailSender emailSender)
    {
      HttpContext.Session.SetSession("Test", _settings.Value);
      var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");

      ViewBag.Title = _settings.Value.ApplicationTitle;
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
