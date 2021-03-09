using Entities.Contracts;
using System.Collections.Generic;

namespace Repository.Moq
{
    public class ShipmentServiceMoq : IShipmentService
    {
        public void Ship(IAddressInfo info, IEnumerable<ICartItem> items)
        {
            throw new System.NotImplementedException();
        }
    }
}
