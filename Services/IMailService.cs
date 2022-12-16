using _101SendEmailNotificationDoNetCoreWebAPI.Model;
using System.Threading.Tasks;
namespace _101SendEmailNotificationDoNetCoreWebAPI.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        
    }
}
