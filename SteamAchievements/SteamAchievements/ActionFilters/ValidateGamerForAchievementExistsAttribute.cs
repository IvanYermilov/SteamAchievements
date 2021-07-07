using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;

namespace SteamAchievements.ActionFilters
{
    public class ValidateGamerForAchievementExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ValidateGamerForAchievementExistsAttribute(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("POST")) ? true : false; ;
            var id = (Guid)context.ActionArguments["gameId"];
            IEnumerable<DeveloperDto> developers = default;
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/developers/for-game-by/{id}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                developers = JsonConvert.DeserializeObject<IEnumerable<DeveloperDto>>(responseBody);
                if (developers == null)
                {
                    _logger.LogInfo($"Game with id: {id} doesn't have any Developer.");
                    context.Result = new NotFoundResult();
                    await next();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            var developer = developers.FirstOrDefault();
            var game = await _repository.Game.GetGameAsync(developer.Id, id, trackChanges);

            if (game == null)
            {
                _logger.LogInfo($"Game with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
            }
        }
    }
}
