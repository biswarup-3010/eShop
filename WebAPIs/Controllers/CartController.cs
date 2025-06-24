using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopDAL;
using eShopDAL.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : Controller
    {
        #region Signature
        private readonly EShopRepository repo;
        public CartController(EShopRepository repository)
        {
            this.repo = repository;
        }
        #endregion

        #region get cart
        [HttpGet("{userId}")]
        public IActionResult GetCartByUserId(int userId)
        {
            var result = repo.GetCartByUserId(userId);

            if (result == null || !result.Any())
                return NotFound(new { message = "No items found in cart." });

            return Ok(result);
        }

        #endregion

        #region add to cart
        [HttpPost]
        public IActionResult AddToCart([FromBody] CartDto dto)
        {
            var result = repo.AddToCart(dto.UserId, dto.ProductId, dto.Quantity);
            return Ok(new { status = result });
        }
        #endregion

        #region remove item from cart
        [HttpDelete("{cartId}")]
        public IActionResult RemoveFromCart(int cartId)
        {
            bool result = repo.RemoveFromCart(cartId);

            if (!result)
                return NotFound(new { message = "Cart item not found or could not be removed." });

            return Ok(new { message = "Item removed from cart successfully." });
        }

        #endregion
    }
}

