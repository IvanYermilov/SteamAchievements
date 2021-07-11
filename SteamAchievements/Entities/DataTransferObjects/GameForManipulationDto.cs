using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class GameForManipulationDto
    {
        [Required(ErrorMessage = "Game's name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<DeveloperForCreationDto> Developers { get; set; }
    }
}
