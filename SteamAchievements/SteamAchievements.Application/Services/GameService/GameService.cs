﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SteamAchievements.Application.DataTransferObjects.Games;
using SteamAchievements.Application.Services.DeveloperService;
using SteamAchievements.Application.Services.RepositoryManager;
using SteamAchievements.Infrastructure.Contracts;
using SteamAchievements.Infrastructure.Entities.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SteamAchievements.Application.Services.GameService
{
    public class GameService : IGameService
    {
        public Game CurrentGame { get; set; }
        public GameDto CurrentGameDto => _mapper.Map<GameDto>(CurrentGame);
        private readonly IRepositoryManager _repository;
        private readonly IDeveloperService _developerService;
        private readonly ILogger<GameService> _logger;
        private readonly IMapper _mapper;

        public GameService(IRepositoryManager repository, IDeveloperService developerService,
            ILogger<GameService> logger, IMapper mapper)
        {
            _repository = repository;
            _developerService = developerService;   
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Game> GetGameByIdAsync(Guid gameId)
        {
            return await _repository.Game.GetGameByIdAsync(gameId, true);
        }

        public async Task<IEnumerable<GameDto>> GetGamesForUserAsync(string userId)
        {
            var games = await _repository.Game.GetGamesForUserAsync(userId,trackChanges:false);
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

        public async Task<Game> GetGameForDeveloperAsync(Guid developerId, Guid gameId, bool trackChanges)
        {
            var game = await _repository.Game.GetGameAsync(developerId, gameId, trackChanges);
            return game;
        }

        public async Task<IEnumerable<GameDto>> GetGamesForDeveloper()
        {
            var developer = _developerService.CurrentDeveloper;
            var gamesFromDb = await _repository.Game.GetGamesAsync(developer.Id, trackChanges: false);
            var games = _mapper.Map<IEnumerable<GameDto>>(gamesFromDb);
            return games;
        }

        public async Task<GameDto> CreateGameForDeveloper(GameForManipulationDto game)
        {
            var gameEntity = _mapper.Map<Game>(game);
            var developerForGame = _developerService.CurrentDeveloper;
            developerForGame.Games.Add(gameEntity);
            await _repository.SaveAsync();
            var gameToReturn = _mapper.Map<GameDto>(gameEntity);
            return gameToReturn;
        }

        public async void AddGameForDeveloper()
        {
            var developer = _developerService.CurrentDeveloper;
            CurrentGame.Developers.Add(developer);
            await _repository.SaveAsync();
        }

        public async Task PartiallyUpdateGameAsync(GameForPatchDto gameToPatch)
        {
            _mapper.Map(gameToPatch, CurrentGame);
            await _repository.SaveAsync();
        }

        public async void DetachGameForDeveloper(Guid developerId)
        {
            var developer = await _repository.Developer.GetDeveloperAsync(developerId, true);
            CurrentGame.Developers.Remove(developer);
            await _repository.SaveAsync();
        }

        public async void DeleteGameForDeveloper()
        {
            _repository.Game.Delete(CurrentGame);
            await _repository.SaveAsync();
        }

        public GameForPatchDto GetGameForPatch()
        {
            return _mapper.Map<GameForPatchDto>(CurrentGame);
        }

        public bool CheckPatchDocIsNull(JsonPatchDocument<GameForPatchDto> patchDoc)
        {
            if (patchDoc != null) return false;
            _logger.LogError("patchDoc object sent from client is null.");
            return true;
        }
    }
}
