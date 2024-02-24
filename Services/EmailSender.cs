using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace TwitterApi.Services
{
   public class EmailSender : IEmailSender
   {
      private readonly ILogger _logger;
      private readonly EmailOptions _emailOptions; //should be set with user secret manager.

      public EmailSender(ILogger<EmailSender> logger,
         IOptions<EmailOptions> optionsAccessor)
      {
         _logger = logger;
         _emailOptions = optionsAccessor.Value;
      }

      public Task SendEmailAsync(string toEmail, string subject, string message)
      {
         MailMessage msg = new();
         msg.Sender = new(_emailOptions.Email);
         msg.From = new(_emailOptions.Email);
         msg.Subject = subject;
         msg.Body = message;
         msg.IsBodyHtml = true;
         msg.To.Add(toEmail);

         using var smtp = new SmtpClient();
         smtp.Host = _emailOptions.Host;
         smtp.Port = _emailOptions.Port;
         smtp.EnableSsl = true;
         smtp.UseDefaultCredentials = false;
         smtp.Credentials = new NetworkCredential(_emailOptions.Email, _emailOptions.Password);
         smtp.Send(msg);

         _logger.LogInformation("Email sent to {toEmail}", toEmail);

         return Task.CompletedTask;
      }
   }
}
