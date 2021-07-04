using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class AchievementRepository : RepositoryBase<Achievement>, IAchievementRepository
    {
        public AchievementRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
