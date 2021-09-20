using SteamAchievements.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using SteamAchievements.Application.Services.GameService;
using SteamAchievements.Application.Services.RepositoryManager;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateGameExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly IGameService _gameService;
        private readonly ILoggerManager _logger;
        public ValidateGameExistsAttribute(IRepositoryManager repository, IGameService gameService, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
            _gameService = gameService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var gameId = (Guid) context.ActionArguments["gameId"];
            var game = await _repository.Game.GetGameById(gameId, true);
            
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