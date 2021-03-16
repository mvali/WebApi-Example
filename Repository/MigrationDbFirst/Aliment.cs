using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.MigrationDbFirst
{
    public partial class Aliment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
        public string Platform { get; set; }
    }
}
