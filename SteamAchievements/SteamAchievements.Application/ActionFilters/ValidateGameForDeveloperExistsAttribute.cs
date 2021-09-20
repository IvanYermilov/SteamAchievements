using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SteamAchievements.Application.Services.GameService;
using SteamAchievements.Application.Services.RepositoryManager;
using SteamAchievements.Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateGameForDeveloperExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IGameService _gameService;


        public ValidateGameForDeveloperExistsAttribute(IRepositoryManager repository, ILoggerManager logger, IGameService gameService)
        {
            _repository = repository;
            _logger = logger;
            _gameService = gameService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;

            var developerId = (Guid)context.ActionArguments["developerId"];
            var developer = await _repository.Developer.GetDeveloperAsync(developerId, false);

            if (developer == null)
            {
                _logger.LogInfo($"Developer with id: {developerId} doesn't exist in the database.");

                context.Result = new NotFoundResult();
                return;
            }

            var id = (Guid)context.ActionArguments["gameId"];
            var game = await _repository.Game.GetGameAsync(developerId, id, trackChanges);

            if (game == null)
            {
                _logger.LogInfo($"Game with id: {id} doesn't exist in the database.");
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
