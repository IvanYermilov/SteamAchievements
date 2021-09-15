using SteamAchievements.InfoStructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SteamAchievements.InfoStructure.Services;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.InfoStructure.ActionFilters
{
    public class ValidateDeveloperForGameExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly CurrentSessionStateService _currentSessionService;

        public ValidateDeveloperForGameExistsAttribute(ILoggerManager logger, IRepositoryManager repository, CurrentSessionStateService currentSessionService)
        {
            _logger = logger;
            _repository = repository;
            _currentSessionService = currentSessionService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("POST")) ? true : false;
            var id = (Guid)context.ActionArguments["developerId"];
            var developer = await _repository.Developer.GetDeveloperAsync(id, trackChanges);

            if (developer == null)
            {
                _logger.LogInfo($"Developer with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                _currentSessionService.CurrentDeveloper = developer;
                await next();
            }
        }
    }
}
