using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.MigrationDbFirst
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            Prices = new HashSet<Price>();
            ProductQuantities = new HashSet<ProductQuantity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<ProductQuantity> ProductQuantities { get; set; }
    }
}
