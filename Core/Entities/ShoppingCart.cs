namespace Core.Entities;

// Not an EF entity. Stored as JSON in Redis.
public class ShoppingCart
{
    public required string Id { get; set; }

    public List<CartItem> Items { get; set; } = [];
}
