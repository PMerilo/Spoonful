using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Vouchers> Rewards { get; set; }
        public DbSet<MealKit> MealKit { get; set; }

        public DbSet<Recipe> Recipe { get; set; }
    }
}

