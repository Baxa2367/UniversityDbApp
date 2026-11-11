
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UniversityDbApp.Data
{
    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=UniversityDbDesignTime;" +
                "User Id=design;Password=DesignOnly_123!;" +
                "TrustServerCertificate=True;Encrypt=True;"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
