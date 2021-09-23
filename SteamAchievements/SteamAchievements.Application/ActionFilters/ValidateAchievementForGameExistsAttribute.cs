using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SteamAchievements.Application.Services.AchievementsService;
using SteamAchievements.Application.Services.GameService;
using SteamAchievements.Infrastructure.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateAchievementForGameExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IAchievementService _achievementService;
        private readonly IGameService _gameService;

        public ValidateAchievementForGameExistsAttribute(ILoggerManager logger, IAchievementService achievementService,
            IGameService gameService)
        {
            _logger = logger;
            _achievementService = achievementService;
            _gameService = gameService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var gameId = (Guid)context.ActionArguments["gameId"];
            var achievementId = (Guid) context.ActionArguments["id"];
            var game = await _gameService.GetGameByIdAsync(gameId);

            if (game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            var achievement = game.Achievements
                .FirstOrDefault(a => a.Id == achievementId);
            
            if (achievement == null)
            {
                _logger.LogInfo($"Achievement with id: {achievementId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                _achievementService.CurrentAchievement = achievement;
                await next();
            }
        }
    }
}
