using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SteamAchievements.Application.Services.GameService;
using SteamAchievements.Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateGameExistsAttribute : IAsyncActionFilter
    {
        private readonly IGameService _gameService;
        private readonly ILoggerManager _logger;
        public ValidateGameExistsAttribute(IGameService gameService, ILoggerManager logger)
        {
            _logger = logger;
            _gameService = gameService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var gameId = (Guid) context.ActionArguments["gameId"];
            var game = await _gameService.GetGameByIdAsync(gameId);
            
            if (game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            _gameService.CurrentGame = game;
            await next();
        }
    }
}