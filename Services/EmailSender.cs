using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace EventManagerADV.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"E-mail verzonden naar {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}
