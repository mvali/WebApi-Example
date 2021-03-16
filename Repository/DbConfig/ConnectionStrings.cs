using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DbConfig
{
    // used in case we want to have the connection string in main core api without Entity Framework
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}
