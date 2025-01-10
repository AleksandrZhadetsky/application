using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using serverApp.Infrastructure;
using serverApp.Models;

namespace serverApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ILogger<AuthController> logger, TokenProvider tokenProvider, IConfiguration configuration) : ControllerBase
    {
        private readonly ILogger<AuthController> logger = logger;
        private readonly TokenProvider tokenProvider = tokenProvider;
        private readonly IConfiguration configuration = configuration;

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserModel userModel)
        {
            if (userModel != null && IsAdmin(userModel))
            {
                var token = tokenProvider.GenerateToken(userModel);

                return Ok(new
                {
                    token,
                    user = userModel
                });
            }

            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        [Route("identityCheck")]
        public IActionResult IdentityCheck()
        {
            return Ok("hello admin!");
        }

        private bool IsAdmin(UserModel user)
        {
            var adminName = configuration["admin:name"];
            var adminEmail = configuration["admin:email"];
            var adminPassword = configuration["admin:secret"];

            return user.Email == adminEmail && user.Name == adminName && user.Password == adminPassword;
        }
    }
}
