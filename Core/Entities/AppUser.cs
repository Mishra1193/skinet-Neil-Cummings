using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        // add any other custom fields you already have
        public string? FirstName { get; set; }
        public string? LastName  { get; set; }

        // 1-1 relation; optional on signup
        public Address? Address { get; set; }
    }
}
