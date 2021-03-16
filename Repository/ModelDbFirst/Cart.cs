using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Repository.ModelDbFirst
{
    public partial class Cart
    {
        [Key]
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Carts")]
        public virtual Product Product { get; set; }
    }
}
