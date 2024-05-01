using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NurseCourse.Models;

namespace NurseCourse.Data
{
    public partial class DbnurseCourseContext : DbContext
    {
        public DbnurseCourseContext()
        {

        }

        public DbnurseCourseContext(DbContextOptions<DbnurseCourseContext> options)
        : base(options)
        {
        }


        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
                entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.LastUpdate).HasDefaultValueSql("(getdate())");

            });

            

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation).WithOne(p => p.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Person");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
