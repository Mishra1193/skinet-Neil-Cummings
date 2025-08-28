using System.ComponentModel.DataAnnotations;

<<<<<<< HEAD
namespace API.Dtos;

public class AddressDto
{
    [Required]
        public string Line1 { get; set; } = string.Empty;

        public string? Line2 { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string? Street2 { get; set; } // optional

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
}
=======
namespace API.Dtos
{
    public class AddressDto
    {
        [Required] public string Line1 { get; set; } = string.Empty;

        // optional per Sir
        public string? Line2 { get; set; }

        [Required] public string City { get; set; } = string.Empty;
        [Required] public string State { get; set; } = string.Empty;
        [Required] public string PostalCode { get; set; } = string.Empty;
        [Required] public string Country { get; set; } = string.Empty;
    }
}
>>>>>>> 808235488e37d5290f340be4b215e065c4e1be6e
