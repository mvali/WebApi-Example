using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.MigrationDbFirst
{
    public partial class Price
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? Price1 { get; set; }

        public virtual Product Product { get; set; }
    }
}
