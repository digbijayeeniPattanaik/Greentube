using API.Controllers;
using API.Model;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace API.Tests
{
    [TestClass]
    public class UsersController
    {
        private Mock<IEmailSender> _mockEmailSender;
        private Mock<ITokenService> _mockTokenService;
        private Controllers.UsersController _usersController;
        [TestInitialize]
        public void Init()
        {
            _mockEmailSender = new Mock<IEmailSender>();
            _mockTokenService = new Mock<ITokenService>();
            _usersController = new Controllers.UsersController(_mockEmailSender.Object, _mockTokenService.Object);
        }

        [TestMethod]
        public async Task Test_ForgotPassword_ModelInvalid()
        {
            var forgotPassword = new ForgotPassword() { Email = "" };
            _usersController.ModelState.AddModelError("Email", "Email is mandatory and must be properly formatted.");
            var result = await _usersController.ForgotPassword(forgotPassword);

            BadRequestResult actionResult = result as BadRequestResult;
            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Test_ForgotPassword_ModelValid_EmailFailed_BadRequest()
        {
            var forgotPassword = new ForgotPassword() { Email = "Test@gmail.com" };

            _mockEmailSender.Setup(a => a.SendEmailAsync(forgotPassword.Email)).ReturnsAsync(new Outcome<string>("Invalid Email"));
            var result = await _usersController.ForgotPassword(forgotPassword);

            BadRequestObjectResult actionResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.AreEqual("Invalid Email", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Test_ForgotPassword_ModelValid_Successful()
        {
            var forgotPassword = new ForgotPassword() { Email = "Test@gmail.com" };

            _mockEmailSender.Setup(a => a.SendEmailAsync(forgotPassword.Email)).ReturnsAsync(new Outcome<string>() { Result = forgotPassword.Email});
            var result = await _usersController.ForgotPassword(forgotPassword);

            OkObjectResult actionResult = result as OkObjectResult;
            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual("Email sent successfully", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Test_ResetPassword_BadRequest()
        {
            var forgotPassword = new ForgotPassword() { Email = "Test@gmail.com" };

            _mockEmailSender.Setup(a => a.SendEmailAsync(forgotPassword.Email)).ReturnsAsync(new Outcome<string>("Invalid Url"));
            var result = await _usersController.ForgotPassword(forgotPassword);

            BadRequestObjectResult actionResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.AreEqual("Invalid Url", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }


        [TestMethod]
        public async Task Test_ResetPassword_Successful()
        {
            _mockTokenService.Setup(a => a.ValidateToken(It.IsAny<string>())).Returns(new Outcome<string>() { Result = "Test@gmail.com" });
            var result = await _usersController.ResetPassword("mocktoken");

            OkObjectResult actionResult = result as OkObjectResult;
            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual("Test@gmail.com", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
