<<<<<<< HEAD
namespace Core.Entities;

using Microsoft.AspNetCore.Identity;


public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public Address? Address { get; set; }
}
=======
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
>>>>>>> 808235488e37d5290f340be4b215e065c4e1be6e
