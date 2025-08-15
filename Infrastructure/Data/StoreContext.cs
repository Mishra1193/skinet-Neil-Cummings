using Core.Entities;
using Microsoft.EntityFrameworkCore;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: explicit relationships (EF can infer these)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductBrand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.ProductBrandId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductType)
            .WithMany(t => t.Products)
            .HasForeignKey(p => p.ProductTypeId);
    }
}
