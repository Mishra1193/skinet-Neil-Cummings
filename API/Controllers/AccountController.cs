using System.Security.Claims;
using API.Extensions;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// aliases to avoid API.DTOs vs API.Dtos collisions
using AddressDto = API.Dtos.AddressDto;
using RegisterDto = API.DTOs.RegisterDto;

namespace API.Controllers;

public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName  = registerDto.LastName,
            Email     = registerDto.Email,
            UserName  = registerDto.Email
        };

        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return ValidationProblem();
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        if (User?.Identity?.IsAuthenticated != true)
            return NoContent();

        // IMPORTANT: include Address so we can return it
        var user = await signInManager.UserManager.GetUserByEmailWithAddressAsync(User);
        if (user is null) return Unauthorized();

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address = user.Address?.ToDto()
        });
    }

    [HttpGet]
    public IActionResult GetAuthState() =>
        Ok(new { IsAuthenticated = User?.Identity?.IsAuthenticated ?? false });

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<AddressDto>> CreateOrUpdateAddress(AddressDto addressDto)
    {
        var user = await signInManager.UserManager.GetUserByEmailWithAddressAsync(User);
        if (user is null) return Unauthorized();

        if (user.Address is null)
            user.Address = addressDto.ToEntity();
        else
            user.Address.UpdateFromDto(addressDto);

        var result = await signInManager.UserManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest("Problem updating user address");

        return Ok(user.Address.ToDto()); // will never be null here
    }
}
