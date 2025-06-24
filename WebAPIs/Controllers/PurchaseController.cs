using Microsoft.AspNetCore.Mvc;
using eShopDAL;
using eShopDAL.DTOs;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PurchaseController : Controller
    {
        private readonly EShopRepository repo;

        public PurchaseController(EShopRepository repository)
        {
            this.repo = repository;
        }

        #region Get All Purchases
        [HttpGet]
        public JsonResult GetAll()
        {
            var purchases = repo.GetAllPurchases();
            return Json(purchases);
        }
        #endregion

        #region Get Purchase By ID
        [HttpGet("{id}")]
        public JsonResult GetById(int id)
        {
            var purchase = repo.GetPurchaseById(id);
            return Json(purchase);
        }
        #endregion

        #region Add Purchase
        [HttpPost]
        public JsonResult Add([FromBody] PurchaseDto dto)
        {
            var result = repo.AddPurchase(dto);
            return Json(new { status = result ? "Success" : "Failed" });
        }
        #endregion

    }
}
