using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        private readonly SymmetricSecurityKey _key; // Only one key is used to encrypt and decrypt the signature in the token
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public string CreateToken(string email, string resetCode)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("reset_code" , resetCode)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public Outcome<string> ValidateToken(string token)
        {
            var stream = token;
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = handler.ReadJwtToken(stream);
            var result = new Outcome<string>("Invalid token");
            if (securityToken != null && securityToken.Claims != null && securityToken.Claims.Any())
            {
                var email = securityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;

                var expiryClaim = securityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value;
                DateTimeOffset expiryDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToInt64(expiryClaim, CultureInfo.InvariantCulture));

                if (expiryDate >  DateTime.UtcNow)
                    result.Result = email;
            }

            return result;
        }
    }
}
