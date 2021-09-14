using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransferObjects.Achievements
{
    public class AchievementForManipulationDto
    {
        [Column("AchievementId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Achievement's name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
