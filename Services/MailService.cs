using MimeKit;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using _101SendEmailNotificationDoNetCoreWebAPI.Settings;
using _101SendEmailNotificationDoNetCoreWebAPI.Model;
//using System.Net.Mail;
namespace _101SendEmailNotificationDoNetCoreWebAPI.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            //MailMessage email=new MailMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

            string[] multiEmail = mailRequest.ToEmail.Split(',');

            foreach(string emailid in multiEmail)
            {
                email.To.Add(MailboxAddress.Parse(emailid));
            }
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
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
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            //SmtpClient smtp = new SmtpClient();
           //smtp.Host = _mailSettings.Host;
           // smtp.Port = _mailSettings.Port;
           // smtp.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            //smtp.EnableSsl = true;
            //smtp.Send(email);
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

       
    }
}
