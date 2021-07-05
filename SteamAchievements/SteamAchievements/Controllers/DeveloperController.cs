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
    [Route("api/developers")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;


        public DeveloperController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "DeveloperById")]
        public async Task<IActionResult> GetDeveloper(Guid id)
        {
            var developer = await _repository.Developer.GetDeveloperAsync(id, trackChanges: false);
            if (developer == null)
            {
                _logger.LogInfo($"Developer with id: {id} doesn't exist in the database.");

                return NotFound();
            }
            else
            {
                var developerDto = _mapper.Map<DeveloperDto>(developer);
                return Ok(developerDto);
            }
        }

        [HttpGet(Name = "GetDevelopers")]
        public async Task<IActionResult> GetAllDeveloper()
        {
            var developers = await _repository.Developer.GetAllDevelopersAsync(trackChanges: false);
            var developerDto = _mapper.Map<IEnumerable<DeveloperDto>>(developers);
            return Ok(developerDto);
        }

        [HttpPost(Name = "CreateDeveloper")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] DeveloperForCreationDto developer)
        {
            var developerEntity = _mapper.Map<Developer>(developer);

            _repository.Developer.CreateDeveloper(developerEntity);
            await _repository.SaveAsync();

            var developerToReturn = _mapper.Map<DeveloperDto>(developerEntity);

            return CreatedAtRoute("DeveloperById", new { id = developerToReturn.Id }, developerToReturn);
        }
    }
}
