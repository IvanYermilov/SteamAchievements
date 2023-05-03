using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SteamAchievements.Application.Services.DeveloperService;
using SteamAchievements.Application.Services.RepositoryManager;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateDeveloperExistsAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidateDeveloperExistsAttribute> _logger;
        private readonly IDeveloperService _developerService;
        public ValidateDeveloperExistsAttribute(IRepositoryManager repository, 
            ILogger<ValidateDeveloperExistsAttribute> logger, IDeveloperService developerService)
        {
            _logger = logger;
            _developerService = developerService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = (Guid)context.ActionArguments["developerId"];
            var developer = await _developerService.GetDeveloperByIdAsync(id, true);

            if (developer == null)
            {
                _logger.LogInformation($"Developer with id: {id} doesn't exist in the database.");
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
