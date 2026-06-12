using Microsoft.EntityFrameworkCore;

namespace MaterialPlanner.Models;

public partial class MaterialPlannerContext : DbContext
{
    public MaterialPlannerContext()
    {
    }

    public MaterialPlannerContext(DbContextOptions<MaterialPlannerContext> options)
        : base(options)
    {
    }

    public DbSet<Brands> Brands { get; set; }
    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<Materials> Materials { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<MaterialDetails> MaterialDetails { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(
    "Server=.;Database=MaterialPlanner;Trusted_Connection=True;TrustServerCertificate=True"
);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 🖼️ IMAGES → MATERIALDETAILS (esto sí puede ser cascade)
        modelBuilder.Entity<Image>()
            .HasOne(i => i.MaterialDetails)
            .WithMany(m => m.Images)
            .HasForeignKey(i => i.MaterialDetailsId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🧩 RELACIONES SEGURAS (NO BORRAN MATERIALDETAILS)

        modelBuilder.Entity<MaterialDetails>()
            .HasOne(m => m.Brand)
            .WithMany()
            .HasForeignKey(m => m.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MaterialDetails>()
            .HasOne(m => m.Product)
            .WithMany()
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MaterialDetails>()
            .HasOne(m => m.Material)
            .WithMany()
            .HasForeignKey(m => m.MaterialId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MaterialDetails>()
            .HasOne(m => m.Presentation)
            .WithMany()
            .HasForeignKey(m => m.PresentationId)
            .OnDelete(DeleteBehavior.SetNull);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}