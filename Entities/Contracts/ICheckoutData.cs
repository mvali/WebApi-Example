using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface ICheckoutData
    {
        public ICard card { get; set; }
        public IAddressInfo addressInfo { get; set; }
    }

}
