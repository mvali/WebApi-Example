using Entities.Models;
using System.Collections.Generic;

namespace Entities.Contracts
{
    // all Operations to be made with IAliment storage database (in Moq or real Db)
    public interface IAlimentRepository
    {
        // data will be saved on Db only when this function is executed
        bool SaveChanges();

        IEnumerable<IAliment> GetAllAliments();
        IAliment GetAlimentById(int id);
        void CreateAliment(IAliment cmd);
        void UpdateAliment(IAliment cmd);
        bool AlimentMethod(int id, IAliment cmd); // make-it bool to exemplify unit testing
        void DeleteAliment(IAliment cmd);
    }
}
