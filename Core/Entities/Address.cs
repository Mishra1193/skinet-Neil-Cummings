<<<<<<< HEAD
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
=======
namespace Core.Entities
{
    public class Address : BaseEntities
    {
        public required string Line1 { get; set; }
        public string? Line2 { get; set; }              // optional

        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
    }
}
>>>>>>> 808235488e37d5290f340be4b215e065c4e1be6e
