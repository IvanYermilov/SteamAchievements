using AutoMapper;
using SteamAchievements.Application.DataTransferObjects.Users;
using SteamAchievements.Application.Services.RepositoryManager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAchievements.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;


        public UserService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = await _repository.User.GetAllUsersAsync(trackChanges: false);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return usersDto;
        }

    }
}