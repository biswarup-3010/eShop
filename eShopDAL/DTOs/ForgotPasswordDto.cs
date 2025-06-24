using System;
namespace eShopDAL.DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}

