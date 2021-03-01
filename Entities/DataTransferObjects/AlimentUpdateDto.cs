using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class AlimentUpdateDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }
    }
}
