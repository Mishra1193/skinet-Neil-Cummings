// Infrastructure/Data/SeedData/StoreContextSeed.cs
using System.Text.Json;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public static class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, ILogger? logger = null)
    {
        if (!context.ProductBrands.Any() || !context.ProductTypes.Any() || !context.Products.Any())
        {
            // Step up from API -> solution root, then into Infrastructure/Data/SeedData
            var apiDir = Directory.GetCurrentDirectory();                          // ...\skinet\API
            var solutionRoot = Directory.GetParent(apiDir)!.FullName;             // ...\skinet
            var productsPath = Path.GetFullPath(
                Path.Combine(solutionRoot, "Infrastructure", "Data", "SeedData", "products.json"));

            if (!File.Exists(productsPath))
                throw new FileNotFoundException($"Seed file not found at {productsPath}");

            var productsData = await File.ReadAllTextAsync(productsPath);
            var rawProducts = JsonSerializer.Deserialize<List<RawProduct>>(productsData) ?? new List<RawProduct>();
            if (rawProducts.Count == 0)
            {
                logger?.LogWarning("No products found in seed file.");
                return;
            }

            var brands = rawProducts
                .Select(p => p.Brand)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(n => new ProductBrand { Name = n })
                .ToList();

            var types = rawProducts
                .Select(p => p.Type)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(n => new ProductType { Name = n })
                .ToList();

            if (!context.ProductBrands.Any()) context.ProductBrands.AddRange(brands);
            if (!context.ProductTypes.Any())  context.ProductTypes.AddRange(types);
            await context.SaveChangesAsync();

            var brandMap = await context.ProductBrands
                .ToDictionaryAsync(b => b.Name, b => b.Id, StringComparer.OrdinalIgnoreCase);
            var typeMap  = await context.ProductTypes
                .ToDictionaryAsync(t => t.Name, t => t.Id, StringComparer.OrdinalIgnoreCase);

            if (!context.Products.Any())
            {
                var products = rawProducts.Select(p => new Product
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    PictureUrl = p.PictureUrl,
                    QuantityInStock = p.QuantityInStock,
                    ProductBrandId = brandMap[p.Brand],
                    ProductTypeId  = typeMap[p.Type]
                }).ToList();

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
                logger?.LogInformation("Seeded {Count} products", products.Count);
            }
        }
    }

    private sealed record RawProduct(
        string Name, string Description, decimal Price, string PictureUrl, string Type, string Brand, int QuantityInStock);
}
