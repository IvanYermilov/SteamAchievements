using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateDeveloperExistsAuthAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidateDeveloperExistsAuthAttribute> _logger;
        public ValidateDeveloperExistsAuthAttribute(ILogger<ValidateDeveloperExistsAuthAttribute> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.Claims.FirstOrDefault((_ => _.Type == ClaimTypes.NameIdentifier))?.Value;
           
            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Result = new UnauthorizedResult();
                _logger.LogDebug("User not found.");
                return;
            }

            context.ActionArguments["userId"] = userId;
            await next();
        }
    }
}
