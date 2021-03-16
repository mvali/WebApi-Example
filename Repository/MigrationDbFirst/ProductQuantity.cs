using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.MigrationDbFirst
{
    public partial class ProductQuantity
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
