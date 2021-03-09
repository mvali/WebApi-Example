using Entities.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Moq
{
    public class CartServiceMoq : ICartService
    {
        public IEnumerable<ICartItem> Items()
        {
            return new CartMoq().GetAll();
        }

        public double Total()
        {
            var retValue = 0d;
            retValue = Items().Sum(x => x.Price);

            return retValue;
        }
    }
}
