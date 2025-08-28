using System.Security.Claims;
using API.Dtos;                     // ✅ add this
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFound() => NotFound();

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest() => BadRequest("Bad request (test)");

        [HttpGet("unauthorised")]
        public IActionResult GetUnauthorised() => Unauthorized();

        [HttpGet("internalerror")]
        public IActionResult GetInternalError() => throw new Exception("Test exception from BuggyController");

        // 🔹 Validation demo using CreateProductDto
        [HttpPost("validation")]
        public IActionResult Validation([FromBody] CreateProductDto dto)
        {
            return Ok(new { message = "Valid payload" });
        }

        // (Optional) keep your minimal DemoDto test too, or remove it—your call.
        public class DemoDto { [System.ComponentModel.DataAnnotations.Required] public string? Name { get; set; } }

        [HttpPost("validationerror")]
        public IActionResult PostValidationError([FromBody] DemoDto dto)
        {
            return Ok("Valid");
        }

        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok($"Hello {name} with the id of {userId}");
        }
    }
}
