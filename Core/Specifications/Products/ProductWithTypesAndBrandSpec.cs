using Core.Entities;

namespace Core.Specifications.Products
{
    // Single product by Id, includes Brand & Type
    public sealed class ProductWithTypesAndBrandSpec : BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandSpec(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
