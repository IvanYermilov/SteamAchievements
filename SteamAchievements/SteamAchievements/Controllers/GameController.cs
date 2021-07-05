using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using SteamAchievements.ActionFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Controllers
{
    [Route("api/developers/{developerId}/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public GameController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDeveloperForGameExistsAttribute))]
        public async Task<IActionResult> CreateGameForDeveloper(Guid developerId, [FromBody] GameForCreationDto game)
        {
            var gameEntity = _mapper.Map<Game>(game);
            var developerForGame = HttpContext.Items["developer"] as Developer;
            if (developerForGame.Games == null)
            {
                developerForGame.Games = new List<Game>();
            }

            developerForGame.Games.Add(gameEntity);
            await _repository.SaveAsync();

            return Ok();
            //return CreatedAtRoute("GetEmployeeForCompany", new
            //{
            //    companyId,
            //    id = employeeToReturn.Id
            //}, employeeToReturn);
        }
    }
}
