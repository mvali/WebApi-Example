using System.Collections.Generic;

namespace Entities.Contracts
{
    // method Ship() that will allow us to ship a cart's content to the customer.
    public interface IShipmentService
    {
        void Ship(IAddressInfo info, IEnumerable<ICartItem> items);
    }
}
