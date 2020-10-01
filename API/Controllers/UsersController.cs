using System.Threading.Tasks;
using API.Model;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public UsersController(IEmailSender emailSender, ITokenService tokenService)
        {
            _emailSender = emailSender;
            _tokenService = tokenService;
        }


        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody]ForgotPassword forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _emailSender.SendEmailAsync(forgotPassword.Email);

                if (!result.Successful) return BadRequest(result.ErrorMessage);

                return Ok("Email sent successfully");
            }

            return BadRequest();
        }

        [HttpPost("resetPassword/{token}")]
        public async Task<ActionResult> ResetPassword([FromRoute] string token)
        {
            var validate = await Task.FromResult(_tokenService.ValidateToken(token));
            if (validate.Successful)
            {
                LoginByEmailAddress(validate.Result);
                return Ok(validate.Result);
            }
            return Unauthorized("Unauthorised");
        }

        private void LoginByEmailAddress(string email)
        { 
        }

    }
}