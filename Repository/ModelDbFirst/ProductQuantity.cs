using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Repository.ModelDbFirst
{
    public partial class ProductQuantity
    {
        [Key]
        public int Id { get; set; }
        [Column("productId")]
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("ProductQuantities")]
        public virtual Product Product { get; set; }
    }
}
