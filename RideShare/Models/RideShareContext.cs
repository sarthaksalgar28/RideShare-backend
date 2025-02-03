using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RideShare.Models
{
    public partial class RideShareContext : DbContext
    {
        public RideShareContext()
        {
        }

        public RideShareContext(DbContextOptions<RideShareContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactForm> ContactForms { get; set; } = null!;
        public virtual DbSet<Ride> Rides { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("myCon");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactForm>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Route)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                // Removed the foreign key relationship
                // If you want to keep DriverId without enforcing the relationship, you can leave it out
                // If you want to keep it, just don't define the relationship
                // entity.HasOne(d => d.Driver)
                //     .WithMany(p => p.Rides)
                //     .HasForeignKey(d => d.DriverId)
                //     .HasConstraintName("FK_Rides_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CarNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CardLastFour)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}