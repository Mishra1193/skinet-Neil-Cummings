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
