using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteamAchievements.InfoStructure.Entities.Models
{
    public class Game
    {
        [Column("GameId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Game's name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Developer> Developers { get; set; } = new List<Developer>();
    }
}
