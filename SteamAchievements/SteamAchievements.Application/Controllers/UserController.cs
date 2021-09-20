using Microsoft.AspNetCore.Mvc;
using SteamAchievements.Application.Services.UserService;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary> 
        /// Gets the list of all users
        /// </summary> 
        /// <returns>The users list</returns> 
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }
    }
}
