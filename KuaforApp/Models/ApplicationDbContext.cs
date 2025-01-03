﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AIRecommendation> AIRecommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configure relationships
            builder.Entity<Service>()
                .HasOne(s => s.Salon)
                .WithMany(s => s.Services)
                .HasForeignKey(s => s.SalonID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Employee>()
                .HasOne(e => e.Salon)
                .WithMany()
                .HasForeignKey(e => e.SalonID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EmployeeService>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(es => es.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EmployeeService>()
                .HasOne(es => es.Service)
                .WithMany()
                .HasForeignKey(es => es.ServiceID)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure Appointment relationships
            builder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Salon)
                .WithMany()
                .HasForeignKey(a => a.SalonID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

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

            builder.Entity<Employee>()
                .Property(e => e.OpeningHours)
                .HasColumnType("time")
                .HasConversion(timeSpanConverter);

            builder.Entity<Employee>()
                .Property(e => e.ClosingHours)
                .HasColumnType("time")
                .HasConversion(timeSpanConverter);
        }
    }
}
