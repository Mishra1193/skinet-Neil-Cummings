using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
    // GET: api/products?brand=...&type=...&sort=priceAsc|priceDesc|alpha
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] string? brand,
        [FromQuery] string? type,
        [FromQuery] string? sort)
    {
        var products = await repo.GetProductsAsync(brand, type, sort);
        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        repo.AddProduct(product);

        var saved = await repo.SaveChangesAsync();
        if (!saved) return Problem("Failed to save the product.");

        // Returns 201 with a Location header pointing to GET /api/products/{id}
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (product.Id != id)
            return BadRequest("Route id and body id must match.");

        // Prefer checking via repository to avoid relying on a possibly-mismatched method name.
        var existing = await repo.GetProductByIdAsync(id);
        if (existing is null) return NotFound();

        // If you're tracking entities, you can map fields; or trust incoming 'product' and update.
        repo.UpdateProduct(product);

        var saved = await repo.SaveChangesAsync();
        if (!saved) return Problem("Failed to update the product.");

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product is null) return NotFound();

        repo.DeleteProduct(product);

        var saved = await repo.SaveChangesAsync();
        if (!saved) return Problem("Failed to delete the product.");

        return NoContent();
    }

    // GET: api/products/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        => Ok(await repo.GetBrandAsync());

    // GET: api/products/types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        => Ok(await repo.GetTypesAsync());
}
