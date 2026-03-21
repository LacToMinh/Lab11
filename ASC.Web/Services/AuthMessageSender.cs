namespace ASC.Web.Services
{
  public class AuthMessageSender : IEmailSender, ISmsSender
  {
    public Task SendEmailAsync(string email, string subject, string message)
    {
      // Code thực tế gọi API gửi email sẽ nằm ở đây
      return Task.FromResult(0);
    }

    public Task SendSmsAsync(string number, string message)
    {
      // Code thực tế gọi API gửi SMS sẽ nằm ở đây
      return Task.FromResult(0);
    }
  }
}
