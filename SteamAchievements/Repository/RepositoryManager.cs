using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IAchievementRepository _achievementRepository;
        private IDeveloperRepository _developerRepository;
        private IGameRepository _gameRepository;
        private IUserRepository _userRepository;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IAchievementRepository Achievement
        {
            get
            {
                if (_achievementRepository == null)
                    _achievementRepository = new AchievementRepository(_repositoryContext);

                return _achievementRepository;
            }
        }
        public IDeveloperRepository Developer
        {
            get
            {
                if (_developerRepository == null)
                    _developerRepository = new DeveloperRepository(_repositoryContext);

                return _developerRepository;
            }
        }
        public IGameRepository Game
        {
            get
            {
                if (_gameRepository == null)
                    _gameRepository = new GameRepository(_repositoryContext);

                return _gameRepository;
            }
        }
        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_repositoryContext);

                return _userRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}
