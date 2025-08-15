using Core.Entities;

namespace Core.Specifications.Products
{
    // LIST spec: filters + includes + sorting + paging
    public sealed class ProductsWithTypesAndBrandsSpec : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpec(ProductSpecParams specParams)
            : base(p =>
                (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
                (string.IsNullOrEmpty(specParams.Brand)  || p.ProductBrand.Name == specParams.Brand) &&
                (string.IsNullOrEmpty(specParams.Type)   || p.ProductType.Name  == specParams.Type)
            )
        {
            // Includes
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            // Default order (alphabetical)
            ApplyOrderBy(p => p.Name);

            // Sorting
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        ApplyOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        ApplyOrderByDescending(p => p.Price);
                        break;
                    default:
                        // name asc already applied
                        break;
                }
            }

            // Paging
            var skip = specParams.PageSize * (specParams.PageIndex - 1);
            ApplyPaging(skip, specParams.PageSize);
        }

        // SINGLE item spec: by Id (includes brand & type)
        public ProductsWithTypesAndBrandsSpec(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
