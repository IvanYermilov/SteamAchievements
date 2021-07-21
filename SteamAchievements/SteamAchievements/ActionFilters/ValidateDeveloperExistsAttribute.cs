using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SteamAchievements.Services;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.ActionFilters
{
    public class ValidateDeveloperExistsAttribute : IAsyncActionFilter
    {
        private readonly Contracts.IRepositoryManager _repository;
        private readonly Contracts.ILoggerManager _logger;
        private readonly CurrentSessionStateService _currentSessionService;
        public ValidateDeveloperExistsAttribute(IRepositoryManager repository, ILoggerManager logger, CurrentSessionStateService currentSessionService)
        {
            _repository = repository;
            _logger = logger;
            _currentSessionService = currentSessionService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = (Guid)context.ActionArguments["developerId"];
            var developer = await _repository.Developer.GetDeveloperAsync(id, true);

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
