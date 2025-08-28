using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;
// NOTE: Ensure your base type is singular "BaseEntity"
public class Address : BaseEntities
{
    // Required at compile-time (and non-nullable => EF makes column NOT NULL)
    public required string Line1 { get; set; }

    // Optional
    public string? Line2 { get; set; }

    public required string City { get; set; }

    public required string State { get; set; }

    public required string PostalCode { get; set; }

    public required string Country { get; set; }
}