using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KuaforApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Salon> Salons { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configure relationships
            builder.Entity<Service>()
                .HasOne(s => s.Salon)
                .WithMany(s => s.Services)
                .HasForeignKey(s => s.SalonID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TimeSpan properties with value converter
            var timeSpanConverter = new ValueConverter<TimeSpan, TimeSpan>(
                timeSpan => timeSpan,
                timeSpan => TimeSpan.FromTicks(timeSpan.Ticks)
            );

            builder.Entity<Salon>()
                .Property(s => s.OpeningHours)
                .HasColumnType("time")
                .HasConversion(timeSpanConverter);

            builder.Entity<Salon>()
                .Property(s => s.ClosingHours)
                .HasColumnType("time")
                .HasConversion(timeSpanConverter);
        }
    }
}
