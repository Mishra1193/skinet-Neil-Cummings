using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Core.Specifications.Products;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _repo;

        public ProductsController(IGenericRepository<Product> repo)
        {
            _repo = repo;
        }

        // GET: api/products
        // e.g. /api/products?brand=Angular&type=Boards&sort=priceDesc&pageIndex=2&pageSize=6&search=board
        [HttpGet]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            // list spec (filters + includes + sort + paging)
            var spec = new ProductsWithTypesAndBrandsSpec(specParams);

            // count spec (filters only, no paging)
            var countSpec = new ProductsWithFiltersForCountSpec(specParams);

            var totalItems = await _repo.CountAsync(countSpec);
            var products   = await _repo.ListAsync(spec);

            var pagination = new Pagination<Product>(
                pageIndex: specParams.PageIndex,
                pageSize:  specParams.PageSize,
                count:     totalItems,
                data:      products
            );

            return Ok(pagination);
        }

        // GET: api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpec(id);
            var product = await _repo.GetEntityWithSpecAsync(spec);
            if (product is null) return NotFound();
            return Ok(product);
        }

        // GET: api/products/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var brands = await _repo.ListAsync(new ProductBrandsSpec());
            return Ok(brands);
        }

        // GET: api/products/types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var types = await _repo.ListAsync(new ProductTypesSpec());
            return Ok(types);
        }
    }
}
