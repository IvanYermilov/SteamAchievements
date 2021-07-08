using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SteamAchievements.ActionFilters
{
    public class ValidateAchievementExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ValidateAchievementExistsAttribute(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("POST")) ? true : false;
            var id = (Guid) context.ActionArguments["gameId"];
            var achievementId = (Guid)context.ActionArguments["id"];
            IEnumerable<DeveloperDto> developers = default;
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response =
                    await client.GetAsync($"https://localhost:5001/api/developers/for-game-by/{id}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                developers = JsonConvert.DeserializeObject<IEnumerable<DeveloperDto>>(responseBody);
                if (developers.Count().Equals(0) || developers == null)
                {
                    _logger.LogInfo($"Game with id: {id} doesn't have any Developer.");
                    context.Result = new NotFoundResult();
                    return;
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
                return;
            }

            var achievement = await _repository.Achievement.GetAchievementAsync(game.Id, achievementId, trackChanges);

            if (achievement == null)
            {
                _logger.LogInfo($"Achievement with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                context.HttpContext.Items.Add("achievement", achievement);
                await next();
            }
        }
    }
}
