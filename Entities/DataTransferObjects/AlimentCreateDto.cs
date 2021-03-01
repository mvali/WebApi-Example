using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class AlimentCreateDto
    {
        // Id is not needed is created by Db
        //public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }
    }
}
