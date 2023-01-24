using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Spoonful.Settings;


namespace Spoonful.Services
{

    public class EmailService
    {
        //private readonly EmailConfiguration _emailConfig;
        public EmailService()
        {
            //_emailConfig = emailConfig;
        }


        public async Task SendEmailAsync(string ToEmail, string Subject, string Body, List<IFormFile>? Attachments)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("noreply@spoonful.com");
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
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("pgmerilo@gmail.com", "ooiwkizuyqhhrpau");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}