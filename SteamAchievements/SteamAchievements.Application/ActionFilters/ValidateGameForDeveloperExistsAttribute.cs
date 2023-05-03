using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SteamAchievements.Application.Services.DeveloperService;
using SteamAchievements.Application.Services.GameService;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateGameForDeveloperExistsAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidateGameForDeveloperExistsAttribute> _logger;
        private readonly IGameService _gameService;
        private readonly IDeveloperService _developerService;


        public ValidateGameForDeveloperExistsAttribute(ILogger<ValidateGameForDeveloperExistsAttribute> logger
            , IGameService gameService, IDeveloperService developerService)
        {
            _logger = logger;
            _gameService = gameService;
            _developerService = developerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;

            var developerId = (Guid)context.ActionArguments["developerId"];
            var developer = await _developerService.GetDeveloperByIdAsync(developerId, true);

            if (developer == null)
            {
                _logger.LogInformation($"Developer with id: {developerId} doesn't exist in the database.");

                context.Result = new NotFoundResult();
                return;
            }

            var gameId = (Guid)context.ActionArguments["gameId"];
            var game = await _gameService.GetGameForDeveloperAsync(developerId, gameId, trackChanges);


            if (game == null)
            {
                _logger.LogInformation($"Game with id: {gameId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                _gameService.CurrentGame = game;
                await next();
            }
        }
    }
}
