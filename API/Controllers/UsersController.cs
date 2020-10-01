using System.Linq;
using System.Threading.Tasks;
using API.Model;
using API.Services;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns><seealso cref="string"/></returns>
        [HttpPost("forgotPassword")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="token">token</param>
        /// <returns><seealso cref="string"/></returns>
        [HttpPost("resetPassword/{token}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ResetPassword([FromRoute] string token)
        {
            var validate = _tokenService.ValidateToken(token);
            if (validate.Successful)
            {
                await LoginByEmailAddress(validate.Result);
                return Ok(validate.Result);
            }
            return Unauthorized(validate.ErrorMessage);
        }

        private async Task LoginByEmailAddress(string email)
        {
            await Task.FromResult(Ok(email));
        }

    }
}