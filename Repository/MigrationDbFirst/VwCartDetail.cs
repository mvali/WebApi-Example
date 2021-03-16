using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.MigrationDbFirst
{
    public partial class VwCartDetail
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Expr1 { get; set; }
    }
}
