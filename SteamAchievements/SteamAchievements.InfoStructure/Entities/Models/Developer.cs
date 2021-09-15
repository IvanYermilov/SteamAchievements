using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Developer
    {
        [Column("DeveloperId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Developer name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Developer address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for rhe Address is 60 characters.")]
        public string Address { get; set; }
        public string Country { get; set; }
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
