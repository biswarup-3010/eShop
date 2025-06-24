using Microsoft.AspNetCore.Mvc;
using eShopDAL;
using eShopDAL.DTOs;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductImageController : Controller
    {
        private readonly EShopRepository repo;

        public ProductImageController(EShopRepository repository)
        {
            this.repo = repository;
        }

        #region Get Images By ProductId
        [HttpGet("{productId}")]
        public JsonResult GetImagesByProductId(int productId)
        {
            var data = repo.GetImagesByProductId(productId);
            return Json(data);
        }
        #endregion

        #region Add Product Image
        [HttpPost]
        public JsonResult AddImage([FromBody] ProductImageDto dto)
        {
            var result = repo.AddProductImage(dto);
            return Json(result);
        }
        #endregion

        #region Update Product Image
        [HttpPut]
        public JsonResult UpdateImage([FromBody] ProductImageDto dto)
        {
            var result = repo.UpdateProductImage(dto);
            return Json(result);
        }
        #endregion

        #region Delete Product Image
        [HttpDelete("{imageId}")]
        public JsonResult DeleteImage(int imageId)
        {
            var result = repo.DeleteProductImage(imageId);
            return Json(result);
        }
        #endregion
    }
}
