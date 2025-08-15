namespace Core.Entities;

public class ProductType : BaseEntities
{
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
