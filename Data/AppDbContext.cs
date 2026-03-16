using Microsoft.EntityFrameworkCore;
using VinokurnyaWpf.Data;

namespace VinokurnyaWpf.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<CalculationHistory> CalculationHistory => Set<CalculationHistory>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Recipe entity
            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(r => r.Name).IsRequired().HasMaxLength(200);
                entity.Property(r => r.Category).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Subcategory).HasMaxLength(50);
                entity.Property(r => r.Notes).HasMaxLength(5000);
                entity.Property(r => r.ImagePath).HasMaxLength(500);
            });

            // Configure Note entity
            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(n => n.Title).IsRequired().HasMaxLength(200);
                entity.Property(n => n.Content).IsRequired().HasMaxLength(5000);
                entity.Property(n => n.Tags).HasMaxLength(200);
            });

            // Configure CalculationHistory entity
            modelBuilder.Entity<CalculationHistory>(entity =>
            {
                entity.Property(c => c.CalculationType).IsRequired().HasMaxLength(50);
                entity.Property(c => c.ParametersJson).IsRequired();
                entity.Property(c => c.ResultJson).IsRequired();
                entity.Property(c => c.Notes).HasMaxLength(1000);
            });
        }
    }
}