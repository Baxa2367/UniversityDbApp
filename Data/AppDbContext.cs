
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Models;

namespace UniversityDbApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<Teacher> Teachers => Set<Teacher>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Teachers)
                .WithOne(t => t.Department)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Department>()
                .Property(d => d.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Teacher>()
                .Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Teacher>()
                .Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.Email)
                .IsUnique();

            modelBuilder.Entity<Teacher>()
                .Property(t => t.Position)
                .HasMaxLength(100);
        }
    }
}

}
