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
    public class CategoryController : Controller
    {
        #region Signature
        private readonly EShopRepository repo;
        public CategoryController(EShopRepository repository)
        {
            this.repo = repository;
        }
        #endregion

        #region Get All Categories
        [HttpGet]
        public ActionResult<List<CategoryDto>> GetAllCategories()
        {
            var categories = repo.GetAllCategories();
            return Ok(categories);
        }
        #endregion

        #region Get Category By Id
        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetCategoryById(int id)
        {
            var category = repo.GetCategoryById(id);
            if (category == null)
                return NotFound(new { message = "Category not found." });

            return Ok(category);
        }
        #endregion

        #region Add Category
        [HttpPost]
        public ActionResult<bool> AddCategory([FromBody] CategoryDto category)
        {
            bool result = repo.AddCategory(category);
            return Ok(result);
        }
        #endregion

        #region Update Category
        [HttpPut]
        public ActionResult<bool> UpdateCategory([FromBody] CategoryDto category)
        {
            bool result = repo.UpdateCategory(category);
            return Ok(result);
        }
        #endregion

        #region Delete Category
        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteCategory(int id)
        {
            bool result = repo.DeleteCategory(id);
            return Ok(result);
        }
        #endregion
    }
}

