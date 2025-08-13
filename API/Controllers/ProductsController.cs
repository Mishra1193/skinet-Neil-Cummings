using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _storeContext;
        private readonly ILogger<ProductsController> _logger;

 public ProductsController(StoreContext storeContext, ILogger<ProductsController> logger)
    {
        _storeContext = storeContext;
        _logger = logger;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        => await _storeContext.Products.ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _storeContext.Products.FindAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _storeContext.Products.Add(product);
        await _storeContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        _storeContext.Entry(product).State = EntityState.Modified;
        Console.WriteLine($"PUT {id}: Name='{product.Name}', Brand='{product.Brand}', IdInBody={product.Id}");
                _logger.LogInformation("PUT {Id}: Name={Name}, Brand={Brand}", id, product.Name, product.Brand);

        await _storeContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _storeContext.Products.FindAsync(id);
        if (product is null) return NotFound();

        _storeContext.Products.Remove(product);
        await _storeContext.SaveChangesAsync();
        return NoContent();
    }

    private bool ProductExists(int id) => _storeContext.Products.Any(p => p.Id == id);
}
