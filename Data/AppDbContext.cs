
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

        // Дерекқор кестелері
        public DbSet<Department> Departments { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department пен Teacher арасындағы байланыс
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Teachers)
                .WithOne(t => t.Department)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department атауы міндетті
            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Teacher электрондық поштасы міндетті
            modelBuilder.Entity<Teacher>()
                .Property(t => t.Email)
                .IsRequired();

            // Электрондық пошта қайталанбауы керек
            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.Email)
                .IsUnique();
        }
    }
}
