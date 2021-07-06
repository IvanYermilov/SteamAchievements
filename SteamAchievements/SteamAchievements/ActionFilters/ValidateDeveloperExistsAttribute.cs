using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace SteamAchievements.ActionFilters
{
    public class ValidateDeveloperExistsAttribute : IAsyncActionFilter
    {
        private readonly Contracts.IRepositoryManager _repository;
        private readonly Contracts.ILoggerManager _logger;
        public ValidateDeveloperExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = (Guid)context.ActionArguments["id"];
            var developer = await _repository.Developer.GetDeveloperAsync(id, true);

            if (developer == null)
            {
                _logger.LogInfo($"Developer with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("developer", developer);
                await next();
            }
        }
    }
}
