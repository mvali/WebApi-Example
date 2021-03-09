using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    public interface ICartRepository
    {
        IEnumerable<ICartItem> GetAll();
        ICartItem GetById(int id);
    }
}
