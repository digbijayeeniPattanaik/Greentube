using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Outcome<string>> SendEmailAsync(string email)
        {
            var path = _configuration["FilePath"];
            string text = @"Dear player,
In order to log in to your account so you can change your password click the following link:

<URL here pointing to the second endpoint described below>

Note that this link is only valid for 1 hour, after which it expires! 

Happy playing!
Greentube Support Team";

            await File.WriteAllTextAsync(path, text);

            return new Outcome<string>() { Result = "Email sent Successfully" };
        }
    }
}
