using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CI.SER.DTOs;
using CI.SER.Interfaces;
namespace CI.SER
{
  public class Mailjet : IEmail
  {
    public async Task SendAsync(string emailTo, string body, string subject, EmailOptionsDTO options)
    {
      var client = new SmtpClient(options.Host, options.Port)
      {
        Credentials = new NetworkCredential(options.ApiKey, options.ApiKeySecret)
      };

      var message = new MailMessage(options.SenderEmail, emailTo)
      {
        Body = body,
        IsBodyHtml = true,
        Subject = $"[ClockIt] {subject}"
      };

      await client.SendMailAsync(message);
    }
  }
}