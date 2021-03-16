using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Repository.ModelDbFirst
{
    public partial class Aliment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Line { get; set; }
        [Required]
        [StringLength(50)]
        public string Platform { get; set; }
    }
}
