using Microsoft.EntityFrameworkCore;
using TruckingTmsMaui.Core.Entities;

namespace TruckingTmsMaui.Data.Context
{
    // EF Core DbContext for local SQLite storage
    public class TruckingTmsDbContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<DocumentAttachment> DocumentAttachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }

        private readonly string _dbPath;

        // Constructor for DI
        public TruckingTmsDbContext(DbContextOptions<TruckingTmsDbContext> options) : base(options) 
        {
            // This constructor is mainly used for DI in MauiProgram.cs
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "truckingtms.db");
        }
        
        // Constructor for direct usage (e.g., seeding)
        public TruckingTmsDbContext()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "truckingtms.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use local SQLite database
            optionsBuilder.UseSqlite($"Filename={_dbPath}")
                          .UseLazyLoadingProxies(); // Enable lazy loading for navigation properties
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision if needed (SQLite often uses TEXT/REAL, but good practice)
            modelBuilder.Entity<Job>()
                .Property(j => j.RatePerLoad)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Job>()
                .Property(j => j.TotalRevenue)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Job>()
                .Property(j => j.RateLeaserChargesYou)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Job>()
                .Property(j => j.RateYouChargeClient)
                .HasColumnType("decimal(18,2)");

            // Enforce required fields and relationships
            modelBuilder.Entity<DocumentAttachment>()
                .HasOne(da => da.Job)
                .WithMany(j => j.DocumentAttachments)
                .HasForeignKey(da => da.JobId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}