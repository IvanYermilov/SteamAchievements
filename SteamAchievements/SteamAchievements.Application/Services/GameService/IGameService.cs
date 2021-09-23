using Microsoft.AspNetCore.JsonPatch;
using SteamAchievements.Application.DataTransferObjects.Games;
using SteamAchievements.Infrastructure.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Services.GameService
{
    public interface IGameService
    {
        public Game CurrentGame { get; set; }
        public GameDto CurrentGameDto { get; }
        public Task<Game> GetGameByIdAsync(Guid gameId);
        public Task<Game> GetGameForDeveloperAsync(Guid developerId, Guid gameId, bool trackChanges);
        public Task<IEnumerable<GameDto>> GetGamesForDeveloper();
        public Task<GameDto> CreateGameForDeveloper(GameForManipulationDto game);
        public void AddGameForDeveloper();
        public Task PartiallyUpdateGameAsync(GameForPatchDto gameToPatch);
        public void DetachGameForDeveloper(Guid developerId);
        public void DeleteGameForDeveloper();
        public GameForPatchDto GetGameForPatch();
        public bool CheckPatchDocIsNull(JsonPatchDocument<GameForPatchDto> patchDoc);
    }
}