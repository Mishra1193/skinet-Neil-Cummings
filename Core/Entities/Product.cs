namespace Core.Entities;

public class Product : BaseEntities
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }

    // FK + navigation to Brand
    public int ProductBrandId { get; set; }
    public ProductBrand ProductBrand { get; set; } = default!;

    // FK + navigation to Type
    public int ProductTypeId { get; set; }
    public ProductType ProductType { get; set; } = default!;

    public int QuantityInStock { get; set; }
}
