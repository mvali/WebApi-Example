using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DbConfig
{
    class SqlProcedure : DbContext
    {
        public SqlProcedure()
        {

        }

        // new SqlParameter("MerchantID", MerchantID ?? SqlInt32.Null)

        public List<T> SqlProcedureGet<T>(string[] param)
        {
            List<T> retVal = default;
            return retVal;
        }
    }
}
