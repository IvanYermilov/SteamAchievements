using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SteamAchievements.Application.Services.DeveloperService;
using SteamAchievements.Application.Services.GameService;
using SteamAchievements.Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateGameForDeveloperExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IGameService _gameService;
        private readonly IDeveloperService _developerService;


        public ValidateGameForDeveloperExistsAttribute(ILoggerManager logger, IGameService gameService,
            IDeveloperService developerService)
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
                _logger.LogInfo($"Developer with id: {developerId} doesn't exist in the database.");

                context.Result = new NotFoundResult();
                return;
            }

            var gameId = (Guid)context.ActionArguments["gameId"];
            var game = await _gameService.GetGameForDeveloperAsync(developerId, gameId, trackChanges);


            if (game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in the database.");
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
