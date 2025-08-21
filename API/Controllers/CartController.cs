using Microsoft.AspNetCore.Mvc;
using Core.Interfaces; // ICartService namespace
using Core.Entities;   // ShoppingCart namespace

namespace API.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: /api/cart?id={id}
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById([FromQuery] string id)
        {
            var cart = await _cartService.GetCartAsync(id);
            return Ok(cart ?? new ShoppingCart { Id = id });
        }

        // POST: /api/cart
        // Body: ShoppingCart JSON
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart([FromBody] ShoppingCart cart)
        {
            var updatedCart = await _cartService.SetCartAsync(cart);

            if (updatedCart is null)
                return BadRequest("Problem with cart.");

            return Ok(updatedCart);
        }

        // DELETE: /api/cart?id={id}
        [HttpDelete]
        public async Task<ActionResult> DeleteCart([FromQuery] string id)
        {
            var result = await _cartService.DeleteCartAsync(id);

            if (!result)
                return BadRequest("Problem deleting cart.");

            return Ok();
        }
    }
}
