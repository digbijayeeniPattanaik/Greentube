using System.Threading.Tasks;
using API.Model;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public UsersController(IEmailSender emailSender)
        {
            this._emailSender = emailSender;
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
    }
}