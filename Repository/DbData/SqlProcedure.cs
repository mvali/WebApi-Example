using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DbData
{
    class SqlProcedure : DbContext
    {
        public SqlProcedure()
        {

        }

        public List<T> SqlProcedureGet<T>(string[] param)
        {
            List<T> retVal = default;
            return retVal;
        }
    }
}
