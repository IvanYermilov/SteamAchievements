using System.Threading.Tasks;
using SteamAchievements.Infrastructure.Contracts;

namespace SteamAchievements.Application.Services.RepositoryManager
{
    public interface IRepositoryManager
    {
        IAchievementRepository Achievement { get; }
        IDeveloperRepository Developer { get; }
        IGameRepository Game { get; }
        IUserRepository User { get; }
        Task SaveAsync();
    }
}
