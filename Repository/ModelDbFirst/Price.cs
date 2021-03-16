using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Repository.ModelDbFirst
{
    public partial class Price
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column("currencyId")]
        public int? CurrencyId { get; set; }
        [Column("Price", TypeName = "decimal(18, 4)")]
        public decimal? Price1 { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty("Prices")]
        public virtual Product Product { get; set; }
    }
}
