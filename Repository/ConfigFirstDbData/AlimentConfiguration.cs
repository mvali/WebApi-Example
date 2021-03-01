using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.ConfigFirstDbData
{
    public class AlimentConfiguration : IEntityTypeConfiguration<Aliment>
    {
        public void Configure(EntityTypeBuilder<Aliment> builder)
        {
            // seed Aliment obkect with data
            builder.HasData
            (
                new Aliment { Id = 1, Name = "Aliment Action1", Line = "Morning 9am", Platform = "Kitchen" },
                new Aliment { Id = 2, Name = "Aliment Action2", Line = "Morning 10am", Platform = "Kitchen" }
            );
        }
    }
}
