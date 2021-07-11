using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Newtonsoft.Json;

namespace SteamAchievements.ActionFilters
{
    public class ValidateGameExistsAttribute : IAsyncActionFilter
    {
        private readonly Contracts.IRepositoryManager _repository;
        private readonly Contracts.ILoggerManager _logger;
        public ValidateGameExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid developerId;
            var gameId = (Guid) context.ActionArguments["id"];
            var method = context.HttpContext.Request.Method;
            IEnumerable<DeveloperDto> developers = default;
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/developers/for-game-by/{gameId}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                developers = JsonConvert.DeserializeObject<IEnumerable<DeveloperDto>>(responseBody);
                if (developers.Count().Equals(0) || developers == null)
                {
                    _logger.LogInfo($"Game with id: {gameId} doesn't have any Developer.");
                    context.Result = new NotFoundResult();
                    return;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            developerId = developers.FirstOrDefault().Id;
            var game = await _repository.Game.GetGameAsync(developerId, gameId, true);
            
            if (game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                context.HttpContext.Items.Add("game", game);
                await next();
            }

        }
    }
}