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
    public class AuthController : Controller
    {
        #region Signature
        private readonly EShopRepository repo;
        public AuthController(EShopRepository repository)
        {
            this.repo = repository;
        }
        #endregion

        #region Login User

        [HttpPost]
        public JsonResult LoginUser([FromBody] LoginDto login)
        {
            var result = repo.LoginUserUsingSP(login.Email, login.Password);

            return Json(new
            {
                Status = result.Status,
                User = result.User
            });
        }

        #endregion

        #region forget password
        [HttpPost]
        public JsonResult ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            string result = repo.ForgotPassword(model.Email, model.NewPassword);

            return Json(new
            {
                Status = result
            });
        }
        #endregion


    }
}

