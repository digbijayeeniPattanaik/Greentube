using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public EmailSender(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }
        public async Task<Outcome<string>> SendEmailAsync(string email)
        {
            var path = _configuration["FilePath"];

            var token = _tokenService.CreateToken(email , RandomString(10));
            var loginUrl = string.Format("{0}{1}{2}", _configuration["ApiUrl"] , "resetPassword/" , token);
            string text = string.Format(@"Dear player,
In order to log in to your account so you can change your password click the following link:

{0}

Note that this link is only valid for 1 hour, after which it expires! 

Happy playing!
Greentube Support Team", loginUrl);

            await File.WriteAllTextAsync(path, text);

            return new Outcome<string>() { Result = "Email sent Successfully" };
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
