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
    public class OrderController : Controller
    {
        #region Signature
        private readonly EShopRepository repo;
        public OrderController(EShopRepository repository)
        {
            this.repo = repository;
        }
        #endregion

        #region Get All Orders
        [HttpGet]
        public ActionResult<List<OrderDto>> GetAllOrders()
        {
            var orders = repo.GetAllOrders();
            return Ok(orders);
        }
        #endregion

        #region Get Order By Id
        [HttpGet("{id}")]
        public ActionResult<OrderDto> GetOrderById(int id)
        {
            var order = repo.GetOrderById(id);
            if (order == null)
                return NotFound(new { message = "Order not found." });

            return Ok(order);
        }
        #endregion

        #region Add Order
        [HttpPost]
        public ActionResult<bool> AddOrder([FromBody] OrderDto dto)
        {
            bool result = repo.AddOrder(dto);
            return Ok(result);
        }
        #endregion

        #region Update Order
        [HttpPut]
        public ActionResult<bool> UpdateOrder([FromBody] OrderDto dto)
        {
            bool result = repo.UpdateOrder(dto);
            return Ok(result);
        }
        #endregion

        #region Delete Order
        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteOrder(int id)
        {
            bool result = repo.DeleteOrder(id);
            return Ok(result);
        }
        #endregion

        #region Get Orders for User
        [HttpGet("{userId}")]
        public ActionResult<List<OrderDto>> GetOrdersByUser(int userId)
        {
            var result = repo.GetOrdersByUser(userId);
            return Ok(result);
        }
        #endregion

        #region Get Order Details by OrderId
        [HttpGet("{orderId}")]
        public JsonResult GetOrderDetailsByOrderId(int orderId)
        {
            var data = repo.GetOrderDetailsByOrderId(orderId);
            return Json(data);
        }
        #endregion
    }
}

