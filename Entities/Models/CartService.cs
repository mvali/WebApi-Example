using Entities.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Entities.Models
{
    public class CartService : ICartService
    {
        public IEnumerable<ICartItem> Items()
        {
            var retValue = new List<ICartItem>() {
                new CartItem(){ ProductId=1, Quantity=5, Price=200d},
                new CartItem(){ProductId=2, Quantity=10, Price=125}
            };
            return retValue;
        }

        public double Total()
        {
            var retValue = 0d;
            retValue = Items().Sum(x => x.Price);

            return retValue;
        }
    }
}
