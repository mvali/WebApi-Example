using Entities.Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using System.Collections.Generic;

namespace Repository.Moq
{
    public class AlimentMoq : IAlimentRepository
    {
        // Mock - comes from fake data
        // Implementation of ICommanderRepo
        // No data connection, contains fake data for testing purposes
        public IEnumerable<IAliment> GetAllAliments()
        {
            var commands = new List<Aliment>()
            {
                new Aliment() { Id = 0, Name = "Cheeze",    Line = "lacto",  Platform = "cow" },
                new Aliment() { Id = 1, Name = "Cranberry", Line = "fruit",  Platform = "bush" },
                new Aliment() { Id = 2, Name = "Pear",      Line = "fruit",  Platform = "tree" }
            };
            return commands;
        }

        public IAliment GetAlimentById(int id)
        {
            return new Aliment() { Id = 0, Name = "Milk", Line = "lacto", Platform = "cow" };
        }   

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateAliment(IAliment cmd)
        {
            throw new System.NotImplementedException();
        }
        public bool AlimentMethod(int id, IAliment cmd)
        {
            throw new System.NotImplementedException();// not now
        }

        public void CreateAliment(IAliment cmd)
        {
            throw new System.NotImplementedException();// not now
        }

        public void DeleteAliment(IAliment cmd)
        {
            throw new System.NotImplementedException();// not now
        }

        // not used, placed just to exemplify the PUT entry object
        public AlimentPatch JsonPatchDocument_AlimentUpdateDto()
        {
            return new AlimentPatch() { op = "replace", path = "/line", value = "mok test value" };
        }

    }
    public class AlimentPatch
    {
        public string op { get; set; }
        public string path { get; set; }
        public string value { get; set; }
    }
}
/* // switch data between tables with/out identity
 CREATE TABLE Test
 (
   id int identity(1,1),
   somecolumn varchar(10)
 );
 INSERT INTO Test VALUES ('Hello');
 INSERT INTO Test VALUES ('World');

 -- copy the table. use same schema, but no identity
 CREATE TABLE Test2
 (
   id int NOT NULL,
   somecolumn varchar(10)
 );

 ALTER TABLE Test SWITCH TO Test2;

 -- drop the original (now empty) table
 DROP TABLE Test;

 -- rename new table to old table's name
 EXEC sp_rename 'Test2','Test';

 -- update the identity seed
 DBCC CHECKIDENT('Test');

 -- see same records
 SELECT * FROM Test;
 */ 