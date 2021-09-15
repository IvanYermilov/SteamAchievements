using System.Threading.Tasks;

namespace SteamAchievements.Infrastructure.Contracts
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
