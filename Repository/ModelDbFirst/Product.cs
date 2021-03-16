using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Repository.ModelDbFirst
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            Prices = new HashSet<Price>();
            ProductQuantities = new HashSet<ProductQuantity>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }

        [InverseProperty(nameof(Cart.Product))]
        public virtual ICollection<Cart> Carts { get; set; }
        [InverseProperty(nameof(Price.Product))]
        public virtual ICollection<Price> Prices { get; set; }
        [InverseProperty(nameof(ProductQuantity.Product))]
        public virtual ICollection<ProductQuantity> ProductQuantities { get; set; }
    }
}
