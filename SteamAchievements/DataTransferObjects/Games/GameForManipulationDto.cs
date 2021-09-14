using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataTransferObjects.Developers;

namespace DataTransferObjects.Games
{
    public class GameForManipulationDto
    {
        [Required(ErrorMessage = "Game's name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<DeveloperForManipulationDto> Developers { get; set; }
    }
}
