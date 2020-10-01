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
        private readonly Random _random = new Random();
        private readonly string _filePath;

        public EmailSender(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _filePath = _configuration["FilePath"];
        }
        public async Task<Outcome<string>> SendEmailAsync(string email)
        {
            //Check if the EmailAddress is valid if connected to Db else return new Outcome<string>("Invalid Email Address");
            string text = GetEmailMessage(email);

            await File.WriteAllTextAsync(_filePath, text);

            return new Outcome<string>() { Result = "Email sent Successfully" };
        }

        private string GetEmailMessage(string email)
        {
            var token = _tokenService.CreateToken(email, RandomString(5));
            var loginUrl = string.Format("{0}{1}", _configuration["resetPasswordUrl"], token);
            string text = string.Format(@"Dear player,
In order to log in to your account so you can change your password click the following link:

{0}

Note that this link is only valid for 1 hour, after which it expires! 

Happy playing!
Greentube Support Team", loginUrl);
            return text;
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
