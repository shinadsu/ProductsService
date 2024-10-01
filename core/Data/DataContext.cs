using core.Models;
using Microsoft.EntityFrameworkCore;

namespace core.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }


        public DbSet<Product> Products { get; set; }

        public DbSet<ProductVersion> ProductVersions { get; set; }

        public DbSet<EventLog> EventLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            // уникальное наименование продукта 
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Versions)
                .WithOne()
                .HasForeignKey(v => v.ProductID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
