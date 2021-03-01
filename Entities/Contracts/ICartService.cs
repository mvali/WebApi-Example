using System.Collections.Generic;

namespace Entities.Contracts
{
    // is a representation of a shopping cart and is able to tell us:
    //      - what is in the cart through the method Items()     and
    //      - how to calculate its total value through the method  Total()
    public interface ICartService
    {
        double Total();
        IEnumerable<ICartItem> Items();
    }
}
