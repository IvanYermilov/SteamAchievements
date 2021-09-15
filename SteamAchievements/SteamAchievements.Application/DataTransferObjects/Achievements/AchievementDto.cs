using System;

namespace SteamAchievements.Application.DataTransferObjects.Achievements
{
    public class AchievementDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
