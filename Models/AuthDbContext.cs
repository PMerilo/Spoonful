using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Spoonful.Models
{
    public class AuthDbContext : IdentityDbContext<CustomerUser>
    {
        private readonly IConfiguration _configuration;
        //public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Category> Category { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Vouchers> Rewards { get; set; }
        public DbSet<MealKit> MealKit { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<AdminDetails> AdminDetails { get; set; }
        public DbSet<DriverDetails> DriverDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserDetails>()
                .ToTable("UserDetails")
                .HasDiscriminator<string>("UserType")
                .HasValue<CustomerDetails>("Customer")
                .HasValue<AdminDetails>("Admin")
                .HasValue<DriverDetails>("Driver"); ;
        }
    }
}

