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
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); 
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DbSet<Category> Category { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Vouchers> Rewards { get; set; }
        public DbSet<MealKit> MealKit { get; set; }
        public DbSet<VoucherEmails> VoucherEmail { get; set; }
        public DbSet<Stops> Stops { get; set; }
        public DbSet<Routes> Route { get; set; }
        public DbSet<Delivery> Delivery { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Recipe> Recipe { get; set; }

        public DbSet<Invoice> Invoice { get; set; }

        public DbSet<Blog> Blog { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Diary> Diary { get; set; }
        public DbSet<ShoppingEntry> Shopping { get; set; }

        public DbSet<Feedbackform> Feedback { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<MailSubscription> mailsubsciption { get; set; }
        public DbSet<TicketingModel> ticketingss { get; set; }

        public DbSet<TixMod> tired { get; set; }

        public DbSet<ProblemThread> Problem { get; set; }








        //Logs

        public DbSet<MealKitSubscriptionLog> MealKitSubscriptionLog { get; set; }

        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<AdminDetails> AdminDetails { get; set; }
        public DbSet<DriverDetails> DriverDetails { get; set; }
        public DbSet<Messages> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserDetails>()
                .ToTable("UserDetails")
                .HasDiscriminator<string>(u => u.UserType)
                .HasValue<CustomerDetails>("Customer")
                .HasValue<AdminDetails>("Admin")
                .HasValue<DriverDetails>("Driver");

			modelBuilder.Entity<Followers>()
	            .HasKey(e => new { e.FollowingId, e.FollowerId });

			modelBuilder.Entity<Followers>()
				.HasOne(e => e.Follower)
				.WithMany(e => e.Followings)
				.HasForeignKey(e => e.FollowerId);

			modelBuilder.Entity<Followers>()
				.HasOne(e => e.Following)
				.WithMany(e => e.Followers)
				.HasForeignKey(e => e.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Messages>()
                .HasKey(e => new { e.Id });

            modelBuilder.Entity<Messages>()
                .HasOne(e => e.Sender)
                .WithMany(e => e.Received)
                .HasForeignKey(e => e.SenderId);

            modelBuilder.Entity<Messages>()
                .HasOne(e => e.Receiver)
                .WithMany(e => e.Sent)
                .HasForeignKey(e => e.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

