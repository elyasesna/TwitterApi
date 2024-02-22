using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TwitterApi.Services
{
   public class EmailSender : IEmailSender
   {
      private readonly ILogger _logger;

      public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                         ILogger<EmailSender> logger)
      {
         Options = optionsAccessor.Value;
         _logger = logger;
      }

      public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

      public async Task SendEmailAsync(string toEmail, string subject, string message)
      {
         if (string.IsNullOrEmpty(Options.SendGridKey))
         {
            throw new Exception("Null SendGridKey");
         }
         await Execute(Options.SendGridKey, subject, message, toEmail);
      }

      public async Task Execute(string apiKey, string subject, string message, string toEmail)
      {
         var client = new SendGridClient(apiKey);
         var from_email = new EmailAddress("your email address", "your name");
         var to_email = new EmailAddress(toEmail, toEmail);
         var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, message, message);
         var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
         _logger.LogInformation(response.IsSuccessStatusCode
                                ? $"Email to {toEmail} queued successfully!"
                                : $"Failure Email to {toEmail}");

         //sending emal using System.Net.Mail
         //MailMessage msg = new();
         //msg.Sender = new("your email address");
         //msg.From = new("your email address");
         //msg.To.Add(toEmail);
         //msg.Subject = subject;
         //msg.Body = message;

         //using var smtp = new SmtpClient();
         //smtp.Host = "smtp.gmail.com";
         //smtp.Port = 587;
         //smtp.EnableSsl = true;
         //smtp.UseDefaultCredentials = false;
         //smtp.Credentials = new NetworkCredential("your email address", "your password");
         //smtp.Send(msg);
      }
   }
}
