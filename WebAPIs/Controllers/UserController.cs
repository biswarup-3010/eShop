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
    public class UserController : Controller
    {
        #region Signature
        private readonly EShopRepository repo;
        public UserController(EShopRepository repository)
        {
            this.repo = repository;
        }
        #endregion

        #region get all users

        [HttpGet]
        public JsonResult GetAllUsers()
        {
            return Json(this.repo.GetAllUsers());
        }

        #endregion

        #region get user

        [HttpGet]
        public JsonResult GetUser(int id)
        {
            return Json(this.repo.GetUser(id));
        }

        #endregion

        #region Register new User
        [HttpPost]
        public JsonResult RegisterUser([FromBody] RegisterDto dto)
        {
            string result = repo.RegisterUser(dto);
            return Json(new
            {
                Status = result
            });
        }

        #endregion

        #region Update user
        [HttpPut]
        public JsonResult UpdateUser([FromBody] UserDto dto)
        {
            string status = repo.UpdateUser(dto);

            return Json(new
            {
                Status = status
            });
        }


        #endregion

        #region Delete User
        [HttpDelete("{id}")]
        public JsonResult DeleteUser(int id)
        {
            bool success = repo.DeleteUser(id);
            return Json(new
            {
                Status = success ? "Success" : "User Not Found or Error"
            });
        }
        #endregion

        #region Get Role by ID
        [HttpGet("{id}")]
        public JsonResult GetRoleById(int id)
        {
            var role = repo.GetRoleById(id);
            if (role == null)
            {
                return Json("Role not found");
            }

            return Json(role);
        }
        #endregion
    }
}

