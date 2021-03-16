using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Repository.ModelDbFirst
{
    [Keyless]
    public partial class VwCartDetail
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public int? Expr1 { get; set; }
    }
}
