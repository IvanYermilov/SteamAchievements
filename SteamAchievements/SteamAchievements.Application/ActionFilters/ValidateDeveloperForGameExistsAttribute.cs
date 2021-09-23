using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SteamAchievements.Application.Services.DeveloperService;
using SteamAchievements.Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateDeveloperForGameExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IDeveloperService _developerService;

        public ValidateDeveloperForGameExistsAttribute(ILoggerManager logger, IDeveloperService developerService)
        {
            _logger = logger;
            _developerService = developerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("POST")) ? true : false;
            var id = (Guid)context.ActionArguments["developerId"];
            var developer = await _developerService.GetDeveloperByIdAsync(id, true);

            if (developer == null)
            {
                _logger.LogInfo($"Developer with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }
            else
            {
                _developerService.CurrentDeveloper = developer;
                await next();
            }
        }
    }
}
