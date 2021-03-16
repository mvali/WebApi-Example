using Entities.Contracts;
using Entities.Models;
using Repository.DbConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    class AlimentRepositoryFromBase : RepositoryBase<Aliment>, IAlimentRepository
    {
        //for getting db connection via dependendecy injection
        // only "base"(RepositoryBase) will have the connection to DB set
        public AlimentRepositoryFromBase(RepositoryContext context) : base(context)
        {
        }

        public IEnumerable<Aliment> GetAliments(string line, bool trackChanges) =>
            FindByCondition(e => e.Line.Equals(line), trackChanges)
            .OrderBy(e => e.Line)
            .ToList();

        public Aliment GetAliment(string line, int id, bool trackChanges) =>
            FindByCondition(e => e.Line.Equals(line) && e.Id.Equals(id), trackChanges)
            .SingleOrDefault();


        public bool AlimentMethod(int id, IAliment cmd)
        {
            return cmd != null && id > 0;
        }

        public void CreateAliment(IAliment cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            // will call base parent method. "base" prefix is not required
            base.Create( (Aliment)cmd);
        }

        public void DeleteAliment(IAliment cmd)
        {
            if (cmd == null)
                throw new NotImplementedException();
            Delete((Aliment)cmd);
        }

        public IEnumerable<IAliment> GetAllAliments()
        {
            return FindAll(false).ToList();
        }

        public IAliment GetAlimentById(int id)
        {
            // it's the same comparison executed twice i=differently, "==" checks for null before executing and does not give error like Equals() do
            return FindByCondition(x => x.Id == id &&  x.Id.Equals(id), false).SingleOrDefault();
        }

        public Aliment GetAlimentByLine(string line)
        {
            return FindByCondition(x => x.Line != null && x.Line.Equals(line), false).SingleOrDefault();
        }

        // no need for it as it is declared in base class (it is not overriden here)
        public new bool SaveChanges()
        {
            // tell if changes in database where successfully saved
            return base.SaveChanges();
        }

        public void UpdateAliment(IAliment cmd)
        {
            // nohing to do here for now, actions made by SaveChanges
            // "base" prefix is not required
            base.Update((Aliment)cmd);
        }
    }
}
