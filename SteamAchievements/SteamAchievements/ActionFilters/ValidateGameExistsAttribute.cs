using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.Models;
using Newtonsoft.Json;

namespace SteamAchievements.ActionFilters
{
    public class ValidateGameExistsAttribute : IAsyncActionFilter
    {
        private readonly Contracts.IRepositoryManager _repository;
        private readonly Contracts.ILoggerManager _logger;
        public ValidateGameExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid developerId;
            var gameId = (Guid) context.ActionArguments["id"];
            var game = await _repository.Game.GetGameById(gameId, true);
            
            if (game == null)
            {
                _logger.LogInfo($"Game with id: {gameId} doesn't exist in the database.");
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