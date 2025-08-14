using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

// Primary constructor injects StoreContext
public sealed class ProductRepository(StoreContext context) : IProductRepository
{
     public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand = null,
                                                               string? type  = null,
                                                               string? sort  = null)
    {
        IQueryable<Product> query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(p => p.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.Type == type);

        // Default to Name when sort is null/empty/unknown
        query = sort switch
        {
            "priceAsc"  => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _           => query.OrderBy(x => x.Name)
        };

        return await query.AsNoTracking().ToListAsync();
    }
    
     public async Task<Product?> GetProductByIdAsync(int id)
      => await context.Products.FindAsync(id);

    public void AddProduct(Product product)
        => context.Products.Add(product); // tracked as Added

    public void UpdateProduct(Product product)
        => context.Entry(product).State = EntityState.Modified; // mark as Modified

    public void DeleteProduct(Product product)
        => context.Products.Remove(product); // tracked as Deleted

    public bool ProductExists(int id)
        => context.Products.Any(p => p.Id == id);

    public async Task<bool> SaveChangesAsync()
        => await context.SaveChangesAsync() > 0;

    public async Task<IReadOnlyList<string>> GetBrandAsync()
    {
        return await context.Products.Select(x => x.Brand)
        .Distinct().
        ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products.Select(x => x.Type)
       .Distinct().
       ToListAsync();
    }

    public Task<IReadOnlyList<Product>> GetProductsAsync(string? brand = null, string? type = null)
    {
        throw new NotImplementedException();
    }
}
