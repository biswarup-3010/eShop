using Microsoft.AspNetCore.Mvc;
using eShopDAL;
using eShopDAL.DTOs;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewController : Controller
    {
        #region Constructor
        private readonly EShopRepository repo;

        public ReviewController(EShopRepository repository)
        {
            this.repo = repository;
        }
        #endregion

        #region Get All Reviews
        [HttpGet]
        public JsonResult GetAll()
        {
            return Json(repo.GetAllReviews());
        }
        #endregion

        #region Get Review By UserId
        [HttpGet("{userId}")]
        public JsonResult GetByUser(int userId)
        {
            return Json(repo.GetReviewByUser(userId));
        }
        #endregion

        #region Get Review By ProductId
        [HttpGet("{productId}")]
        public JsonResult GetByProduct(int productId)
        {
            return Json(repo.GetReviewForProduct(productId));
        }
        #endregion

        #region Add Review
        [HttpPost]
        public JsonResult Add([FromBody] ReviewDto dto)
        {
            var result = repo.AddReview(dto);
            return Json(new { status = result ? "Success" : "Failed" });
        }
        #endregion

        #region Update Review
        [HttpPut]
        public JsonResult Update([FromBody] ReviewDto dto)
        {
            var result = repo.UpdateReview(dto);
            return Json(new { status = result ? "Updated" : "Not Found" });
        }
        #endregion

        #region Delete Review
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var result = repo.DeleteReview(id);
            return Json(new { status = result ? "Deleted" : "Not Found" });
        }
        #endregion
    }
}
