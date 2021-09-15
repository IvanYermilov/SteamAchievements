using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Achievement
    {
        [Column("AchievementId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Achievement's name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
        
        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
