using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Spoonful.Settings;

namespace Spoonful.Services
{

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public Task SendEmailAsync(string ToEmail, string Subject, string Body, List<IFormFile>? Attachments)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailConfig.From));
            email.To.Add(MailboxAddress.Parse(ToEmail));
            email.Subject = Subject;
            var builder = new BodyBuilder();
            if (Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = Body;
            email.Body = builder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.Auto);
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

            return Task.CompletedTask;
        }
    }
}