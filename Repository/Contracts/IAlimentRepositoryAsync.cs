using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entities.Contracts
{
    // all Operations to be made with IAliment storage database (in Moq or real Db)
    public interface IAlimentRepositoryAsync
    {
        // data will be saved on Db only when this function is executed
        bool SaveChanges();

        Task <IEnumerable<IAliment>> GetAllAlimentsAsync();
        IAliment GetAlimentById(int id);
        Task<IAliment> GetAlimentByIdAsync(int id);
        Task CreateAlimentAsync(IAliment cmd);
        Task UpdateAlimentAsync(IAliment cmd);
        Task<bool> AlimentMethodAsync(int id, IAliment cmd); // make-it bool to exemplify unit testing
        //Task<int> AlimentMethodIntAsync(int id, IAliment cmd); // make-it int to exemplify unit testing
        Task DeleteAlimentAsync(IAliment cmd);
    }
}
