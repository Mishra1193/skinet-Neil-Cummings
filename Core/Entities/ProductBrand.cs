namespace Core.Entities;

public class ProductBrand : BaseEntities
{
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
