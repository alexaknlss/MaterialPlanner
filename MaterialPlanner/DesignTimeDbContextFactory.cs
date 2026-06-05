using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MaterialPlanner.Models;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MaterialPlannerContext>
{
    public MaterialPlannerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MaterialPlannerContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=MaterialPlanner;Trusted_Connection=True;TrustServerCertificate=True");

        return new MaterialPlannerContext(optionsBuilder.Options);
    }
}
