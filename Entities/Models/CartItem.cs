using Entities.Contracts;

namespace Entities.Models
{
    public class CartItem : ICartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
