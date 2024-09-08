using System.Threading.Tasks;

namespace SpostatoBL.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}