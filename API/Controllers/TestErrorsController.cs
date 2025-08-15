using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestErrorsController : ControllerBase
    {
        [HttpGet("boom")]
        public IActionResult Boom() => throw new Exception("Kaboom!");
    }
}
