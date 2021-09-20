using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Application.ActionFilters;
using SteamAchievements.Application.DataTransferObjects.Users;
using SteamAchievements.Application.Services.AuthenticationService;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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
    }
}
