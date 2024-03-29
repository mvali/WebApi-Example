﻿using Entities.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DbConfig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class AlimentRepository : IAlimentRepository, IAlimentRepositoryAsync
    {
        //for getting db connection via dependendecy injection
        private readonly SqlContext _context; // autogenerated: press "."  on _context: .generate readonly field
        public AlimentRepository(SqlContext context)
        {
            // generate readonly field
            _context = context;
        }
        public bool AlimentMethod(int id, IAliment cmd)
        {
            return cmd != null && id > 0;
        }
        public async Task<bool> AlimentMethodAsync(int id, IAliment cmd)
        {
            bool retvalue = cmd != null && id > 0;
            return AlimentMethod(id, cmd);
        }

        public void CreateAliment(IAliment cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            // command is created but data will be saved when SaveChanges is also called
            _context.Aliments.Add((Aliment)cmd);
        }
        public Task CreateAlimentAsync(IAliment cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            // command is created but data will be saved when SaveChanges is also called
            //_context.Aliments.Add((Aliment)cmd);
            CreateAliment(cmd);

            return Task.CompletedTask;
        }

        public void DeleteAliment(IAliment cmd)
        {
            if (cmd == null)
                throw new NotImplementedException();
            _context.Aliments.Remove((Aliment)cmd);
        }
        public Task DeleteAlimentAsync(IAliment cmd)
        {
            if (cmd == null)
                throw new NotImplementedException();

            //_context.Aliments.Remove((Aliment)cmd);
            DeleteAliment(cmd);

            return Task.CompletedTask;
        }

        public IEnumerable<IAliment> GetAllAliments()
        {
            return _context.Aliments.ToList();
        }
        public async Task<IEnumerable<IAliment>> GetAllAlimentsAsync()
        {
            return await _context.Aliments.ToListAsync();
        }

        public IAliment GetAlimentById(int id)
        {
            return _context.Aliments.FirstOrDefault(x => x.Id == id);
        }
        public async Task<IAliment> GetAlimentByIdAsync(int id)
        {
            return await _context.Aliments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            // tell if changes in database where successfully saved
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateAliment(IAliment cmd)
        {
            // nohing to do here for now, actions made by SaveChanges
        }
        public Task UpdateAlimentAsync(IAliment cmd)
        {
            // nohing to do here for now, actions made by SaveChanges
            return Task.CompletedTask;
        }

        public List<Aliment> AlimentsGet(int id, string param1)
        {
            List<Aliment> retValue = null;

            var parameters = new List<SqlParameter> { 
                new ( "@id", id ),
                new ("@param1", param1)
            };
            // this is available in .NetFramework, not in Core
            //var result = _context.Database.SqlQuery

            return retValue;
        }


        // methods for executing stored procedures in Core:
        //  DbSet<TEntity>.FromSql()
        //  DbContext.Database.ExecuteSqlCommand()
        /*
        1- Create a View from select part of sql query --without where condition--.
        2- Then generate your model from database and Entity Framework generates a model for your view.
        3- Now you can execute your stored procedure using generated model from view
        dbContext.GeneratedView.FromSqlRaw("MyStoredProcedure {0}, {1} ", param1, param2)
         */

        /*public IEnumerable<StudentDetail> GetStudentDetails(int ssid)
        {
            var ssidParam = new SqlParameter("@ssid", ssid);
            var result = _appDbContext.StudentDetails.FromSql("exec GetStudentDetail @ssid", ssidParam).AsNoTracking().ToList();
            return result;
        }*/
    }
}
