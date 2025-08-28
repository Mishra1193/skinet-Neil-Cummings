<<<<<<< HEAD
using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email)
            ?? throw new AuthenticationException("Email claim not found");

        return email;
    }

    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var email = user.GetEmail();

        var userToReturn = await userManager.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (userToReturn == null)
            throw new AuthenticationException("User not found");

        return userToReturn;
    }

    public static async Task<AppUser?> GetUserByEmailWithAddressAsync(
           this UserManager<AppUser> userManager, ClaimsPrincipal user)
    {
        var userToReturn = await userManager.Users
            .Include(u => u.Address)
            .FirstOrDefaultAsync(x => x.Email == user.GetEmail());

        if (userToReturn == null)
            throw new AuthenticationException("User not found");
                
                return userToReturn;
    }
}
=======
using System.Security.Claims;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // Base method (without Address) - useful elsewhere
        public static async Task<AppUser?> GetUserByEmailAsync(
            this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email)) return null;

            return await userManager.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        // ✅ Sir's variant: include Address
        public static async Task<AppUser?> GetUserByEmailWithAddressAsync(
            this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email)) return null;

            return await userManager.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
>>>>>>> 808235488e37d5290f340be4b215e065c4e1be6e
