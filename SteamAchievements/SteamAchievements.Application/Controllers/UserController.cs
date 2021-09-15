using AutoMapper;
using SteamAchievements.InfoStructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataTransferObjects.Users;

namespace SteamAchievements.Application.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;


        public UserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary> 
        /// Gets the list of all users
        /// </summary> 
        /// <returns>The users list</returns> 
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var  users = await _repository.User.GetAllUsersAsync(trackChanges: false);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }
    }
}
