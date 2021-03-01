using Entities.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Aliment : IAliment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }
    }
}
