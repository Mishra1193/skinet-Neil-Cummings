using Core.Entities;
using Core.Specifications;

namespace Core.Specifications.Products
{
    public sealed class ProductTypesSpec : BaseProjectionSpecification<Product, string>
    {
        public ProductTypesSpec()
        {
            ApplySelect(p => p.ProductType.Name);
            ApplyDistinct();
        }
    }
}
