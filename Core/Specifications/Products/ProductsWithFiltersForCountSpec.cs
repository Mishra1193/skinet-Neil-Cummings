using Core.Entities;

namespace Core.Specifications.Products
{
    // Count spec: filters only (no paging)
    public sealed class ProductsWithFiltersForCountSpec : BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpec(ProductSpecParams specParams)
            : base(p =>
                (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
                (string.IsNullOrEmpty(specParams.Brand)  || p.ProductBrand.Name == specParams.Brand) &&
                (string.IsNullOrEmpty(specParams.Type)   || p.ProductType.Name  == specParams.Type)
            )
        {
        }
    }
}
