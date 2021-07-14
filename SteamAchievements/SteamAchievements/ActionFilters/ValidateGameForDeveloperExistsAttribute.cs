using System;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SteamAchievements.ActionFilters
{
    public class ValidateGameForDeveloperExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateGameForDeveloperExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
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
                context.HttpContext.Items.Add("game", game);
                await next();
            }
        }
    }
}
