using Core.Entities;
using Core.Specifications;

namespace Core.Specifications.Products
{
    public sealed class ProductBrandsSpec : BaseProjectionSpecification<Product, string>
    {
        public ProductBrandsSpec()
        {
            ApplySelect(p => p.ProductBrand.Name);
            ApplyDistinct();
        }
    }
}
