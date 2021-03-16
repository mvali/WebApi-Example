using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.MigrationDbFirst
{
    public partial class VwProductsPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
