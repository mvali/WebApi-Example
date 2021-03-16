using Entities.Contracts;
using Entities.Models;
using Repository.DbConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class BrandRepositoryFromBase : RepositoryBase<Aliment>
    {
        //for getting db connection via dependendecy injection
        // only "base"(RepositoryBase) will have the connection to DB set
        public BrandRepositoryFromBase(RepositoryContext context) : base(context)
        {
        }

        // no need for it as it is declared in base class (it is not overriden here)
        public new bool SaveChanges()
        {
            // tell if changes in database where successfully saved
            return base.SaveChanges();
        }

    }
}
