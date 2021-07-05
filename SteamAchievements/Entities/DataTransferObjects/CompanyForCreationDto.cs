using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class DeveloperForCreationDto
    {
        [Required(ErrorMessage = "Developer's name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Developer's address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for rhe Address is 60 characters.")]
        public string Address { get; set; }
        public string Country { get; set; }
        public IEnumerable<GameForCreationDto> Games { get; set; }
    }
}
