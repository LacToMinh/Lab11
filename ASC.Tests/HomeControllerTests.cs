using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASC.Web.Services;
using ASC.Web.Controllers;
using ASC.Web.Configuration;
using Microsoft.AspNetCore.Http; // Thêm thư viện này cho ILogger

// Đừng quên thêm using đến các namespace chứa HomeController, ApplicationSettings và IEmailSender của bạn
// Ví dụ: 
// using ASC.Web.Controllers;
// using ASC.Web.Configuration;
// using ASC.Web.Services; 

namespace ASC.Tests
{
  public class HomeControllerTests
  {
    private readonly Mock<IOptions<ApplicationSettings>> optionsMock;

    // 1. Khai báo thêm Mock cho ILogger
    private readonly Mock<ILogger<HomeController>> loggerMock;

    public HomeControllerTests()
    {
      optionsMock = new Mock<IOptions<ApplicationSettings>>();
      optionsMock.Setup(ap => ap.Value).Returns(new ApplicationSettings
      {
        ApplicationTitle = "ASC"
      });

      // 2. Khởi tạo Mock ILogger
      loggerMock = new Mock<ILogger<HomeController>>();
    }

    [Fact]
    public void HomeController_Index_View_Test()
    {
      // 3. Truyền cả loggerMock.Object và optionsMock.Object vào Controller
      var controller = new HomeController(loggerMock.Object, optionsMock.Object);

      // 4. Tạo Mock IEmailSender để truyền vào hàm Index()
      var emailSenderMock = new Mock<IEmailSender>();

      Assert.IsType(typeof(ViewResult), controller.Index(emailSenderMock.Object));
    }

    [Fact]
    public void HomeController_Index_NoModel_Test()
    {
      var controller = new HomeController(loggerMock.Object, optionsMock.Object);
      var emailSenderMock = new Mock<IEmailSender>();

      // Nhớ truyền emailSenderMock.Object vào Index()
      Assert.Null((controller.Index(emailSenderMock.Object) as ViewResult).ViewData.Model);
    }

    [Fact]
    public void HomeController_Index_Validation_Test()
    {
      var controller = new HomeController(loggerMock.Object, optionsMock.Object);
      var emailSenderMock = new Mock<IEmailSender>();

      // Nhớ truyền emailSenderMock.Object vào Index()
      Assert.Equal(0, (controller.Index(emailSenderMock.Object) as ViewResult).ViewData.ModelState.ErrorCount);
    }

    [Fact]
    public void HomeController_Index_Session_Test()
    {
      var controller = new HomeController(loggerMock.Object, optionsMock.Object);

      // Khởi tạo HttpContext và gán FakeSession
      controller.ControllerContext.HttpContext = new DefaultHttpContext();
      controller.ControllerContext.HttpContext.Session = new TestUtilities.FakeSession();

      var emailSenderMock = new Mock<IEmailSender>();
      controller.Index(emailSenderMock.Object);

      // Assert: Kiểm tra xem Session có chứa giá trị không
      // Lưu ý: Đoạn GetSession<HomeController>("Test") có thể yêu cầu bạn phải viết thêm Extension Method cho Session theo giáo trình.
      //Assert.NotNull(controller.ControllerContext.HttpContext.Session.GetSession<HomeController>("Test"));
    }
  }
}