using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Application.ActionFilters;
using SteamAchievements.Application.DataTransferObjects.Users;
using SteamAchievements.Application.Services.AuthenticationService;
using SteamAchievements.Infrastructure.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService
            , ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForManipulationDto userForRegistration)
        {
            var result = await _authenticationService.RegisterUser(userForRegistration);
            if (result.Any(result => !result.Succeeded))
            {
                foreach (var error in result.SelectMany(resultItem => resultItem.Errors))
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationService.ValidateUser(user))
            {
                _logger.LogWarning($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password."); 
                return Unauthorized();
            }

            return Ok(new { Token = await _authenticationService.CreateToken() });
        }

    }
}
