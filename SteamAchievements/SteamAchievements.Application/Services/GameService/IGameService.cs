using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SteamAchievements.Application.Controllers;
using SteamAchievements.Application.DataTransferObjects.Games;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Application.Services.GameService
{
    public interface IGameService
    {
        public Game CurrentGame { get; set; }
        public GameDto GetGame();
        public Task<IEnumerable<GameDto>> GetGamesForDeveloper();
        public Task<GameDto> CreateGameForDeveloper(GameForManipulationDto game);
        public void AddGameForDeveloper();
        public Task<ModelStateDictionary> PartiallyUpdateGame(JsonPatchDocument<GameForManipulationDto> patchDoc, GameController controller);
        public void DetachGameForDeveloper(Guid developerId);
        public void DeleteGameForDeveloper();
        public bool CheckPatchDocIsNull(JsonPatchDocument<GameForManipulationDto> patchDoc);
    }
}