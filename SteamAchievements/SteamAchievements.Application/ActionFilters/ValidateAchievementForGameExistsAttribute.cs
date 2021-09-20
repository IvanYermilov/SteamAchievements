using AutoMapper;
using SteamAchievements.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using SteamAchievements.Application.Services.AchievementsService;
using SteamAchievements.Application.Services.RepositoryManager;

namespace SteamAchievements.Application.ActionFilters
{
    public class ValidateAchievementForGameExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IAchievementService _achievementService;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ValidateAchievementForGameExistsAttribute(ILoggerManager logger, IRepositoryManager repository, 
            IMapper mapper, IAchievementService achievementService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _achievementService = achievementService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var gameId = (Guid)context.ActionArguments["gameId"];
            var achievementId = (Guid) context.ActionArguments["id"];
            var game = await _repository.Game.GetGameById(gameId, true);

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
