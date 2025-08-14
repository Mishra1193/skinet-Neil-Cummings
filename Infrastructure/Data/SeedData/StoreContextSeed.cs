using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data.SeedData;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext storeContext)
    {
        if (!storeContext.Products.Any())
        {
            var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            var products = JsonSerializer.Deserialize<List<Product>>(productData);

            if (products == null) return;

            storeContext.Products.AddRange(products);
            await storeContext.SaveChangesAsync();
        }
    }
}
