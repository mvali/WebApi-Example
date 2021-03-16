using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.ConfigFirstDbData;

namespace Repository.DbConfig
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // used to seed Aliment object/db with default data
            modelBuilder.ApplyConfiguration(new AlimentConfiguration());
        }

        public DbSet<Aliment> Aliments { get; set; }
        //public DbSet<ICartItem> CartItems { get; set; }

    }
}
