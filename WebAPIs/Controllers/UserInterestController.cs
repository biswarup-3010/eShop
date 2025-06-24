using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopDAL;
using eShopDAL.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace WebAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserInterestController : Controller
    {
        #region Signature
        private readonly EShopRepository repo;
        public UserInterestController(EShopRepository repository)
        {
            this.repo = repository;
        }
        #endregion

        #region get user interests
        [HttpGet("{userId}")]
        public JsonResult GetUserInterests(int userId)
        {
            var interests = repo.GetUserInterests(userId);
            return Json(interests);
        }
        #endregion

        #region add user interest
        [HttpPost]
        public JsonResult AddUserInterest([FromBody] UserInterestDto dto)
        {
            bool result = repo.AddUserInterest(dto);
            return Json(new { success = result });
        }
        #endregion

        #region update user interest
        [HttpPut]
        public JsonResult UpdateUserInterest([FromBody] UserInterestDto dto)
        {
            bool result = repo.UpdateUserInterest(dto);
            return Json(new { success = result });
        }
        #endregion

        #region delete user interest
        [HttpDelete("{interestId}")]
        public JsonResult DeleteUserInterest(int interestId)
        {
            bool result = repo.DeleteUserInterest(interestId);
            return Json(new { success = result });
        }
        #endregion
    }
}

