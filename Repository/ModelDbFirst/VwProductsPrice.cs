using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Repository.ModelDbFirst
{
    [Keyless]
    public partial class VwProductsPrice
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        [Column("currencyId")]
        public int? CurrencyId { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
