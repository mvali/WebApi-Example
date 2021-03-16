using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Repository.ModelDbFirst
{
    public partial class DbContextApi : DbContext
    {
        public DbContextApi()
        {
        }

        public DbContextApi(DbContextOptions<DbContextApi> options)
            : base(options)
        {
        }

        public virtual DbSet<Aliment> Aliments { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductQuantity> ProductQuantities { get; set; }
        public virtual DbSet<VwCartDetail> VwCartDetails { get; set; }
        public virtual DbSet<VwProductsPrice> VwProductsPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=AMDP\\SQLEXPRESS2019;Initial Catalog=WebApiExample;User ID=webapi;Password=webapi;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Aliment>(entity =>
            {
                entity.Property(e => e.Line).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Platform).IsUnicode(false);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Carts_Products");
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Prices)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Prices_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description).IsFixedLength(true);

                entity.Property(e => e.Name).IsFixedLength(true);
            });

            modelBuilder.Entity<ProductQuantity>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductQuantities)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductQuantities_Products");
            });

            modelBuilder.Entity<VwCartDetail>(entity =>
            {
                entity.ToView("vwCartDetails");

                entity.Property(e => e.Description).IsFixedLength(true);

                entity.Property(e => e.Name).IsFixedLength(true);
            });

            modelBuilder.Entity<VwProductsPrice>(entity =>
            {
                entity.ToView("vwProductsPrices");

                entity.Property(e => e.Description).IsFixedLength(true);

                entity.Property(e => e.Name).IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
