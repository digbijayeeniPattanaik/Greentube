using System.Threading.Tasks;

namespace API.Services
{
    public interface IEmailSender
    {
        Task<Outcome<string>> SendEmailAsync(string email);
    }
}
