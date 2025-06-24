using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopDAL;
using eShopDAL.Data;
using eShopDAL.DTOs;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : Controller
    {

        #region Signature
        private readonly EShopRepository repo;

        public ProductController(EShopRepository repo)
        {
            this.repo = repo;
        }

        #endregion

        #region get all products
        [HttpGet]
        public JsonResult GetAllProducts()
        {
            return Json(this.repo.GetAllProducts());
        }
        #endregion

        #region Get product by Id
        [HttpGet]
        public JsonResult GetProductById(int id)
        {
            return Json(this.repo.GetProductById(id));
        }
        #endregion

        #region Add product
        [HttpPost]
        public JsonResult AddProduct([FromBody] ProductDto dto)
        {
            return Json(this.repo.AddProduct(dto));
        }
        #endregion

        #region update product
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            if (id != dto.ProductId)
                return BadRequest("Product ID in URL and body do not match.");

            string status = this.repo.UpdateProductUsingSP(dto);

            if (status == "Success")
                return Ok(new { message = "Product updated successfully." });

            if (status == "Product Not Found")
                return NotFound(new { message = "Product not found." });

            return StatusCode(500, new { message = "Unknown error occurred." });
        }
        #endregion

        #region Delete product
        [HttpDelete("{id}")]
        public JsonResult DeleteProduct(int id)
        {
            bool result = repo.DeleteProduct(id);

            return Json(result);
        }

        #endregion
    }
}

