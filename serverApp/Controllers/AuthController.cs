using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using serverApp.Infrastructure;
using serverApp.Models;

namespace serverApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(ILogger<AuthController> logger, TokenProvider tokenProvider) : ControllerBase
    {
        private readonly ILogger<AuthController> logger = logger;
        private readonly TokenProvider tokenProvider = tokenProvider;

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserModel userModel)
        {
            var token = tokenProvider.GenerateToken(userModel);

            return Ok(new
            {
                token,
                user = userModel
            });
        }

        [HttpGet]
        [Authorize]
        [Route("identityCheck")]
        public IActionResult IdentityCheck()
        {
            return Ok("hello admin!");
        }
    }
}
