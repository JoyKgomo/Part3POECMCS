using Microsoft.EntityFrameworkCore;
using POEPART2CMCSFINAL.Models;
using POEPART2CMCSFINAL.Models.Seeders;
using System;

namespace POEPART2CMCSFINAL.Services
{
    public class ClaimContext : DbContext
    {
        public ClaimContext(DbContextOptions<ClaimContext> options) : base(options)
        {
            // Database.EnsureCreated(); // Ensure tables are created
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Claims-Data2.sqlite");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().HasKey(x => x.ID);
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimConfiguration());

        }
    }
}
