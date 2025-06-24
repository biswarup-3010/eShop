using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using eShopDAL;
using eShopDAL.DTOs;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        #region Constructor

        private readonly EShopRepository repo;

        public WishlistController(EShopRepository repository)
        {
            this.repo = repository;
        }

        #endregion

        #region Get Wishlist by User

        [HttpGet("{userId}")]
        public IActionResult GetUserWishlist(int userId)
        {
            var wishlist = repo.GetUserWishlist(userId);
            return Ok(wishlist);
        }

        #endregion

        #region Add to Wishlist

        [HttpPost]
        public IActionResult AddToWishlist([FromBody] WishlistDto dto)
        {
            var success = repo.AddToWishlist(dto);
            return Ok(new { Status = success ? "Success" : "AlreadyExists" });
        }

        #endregion

        #region Move to Cart

        [HttpPost("{wishlistId}")]
        public IActionResult MoveToCart(int wishlistId)
        {
            var success = repo.MoveWishlistItemToCart(wishlistId);
            return Ok(new { Status = success ? "Success" : "Failed" });
        }

        #endregion

        #region Remove from Wishlist

        [HttpDelete("{wishlistId}")]
        public IActionResult RemoveFromWishlist(int wishlistId)
        {
            var success = repo.RemoveFromWishlist(wishlistId);
            return Ok(new { Status = success ? "Success" : "NotFound" });
        }

        #endregion
    }
}
