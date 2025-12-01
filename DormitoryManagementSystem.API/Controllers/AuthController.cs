using System;
using DormitoryManagementSystem.BUS.Implementations;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Users;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserBUS _userBUS;
        public AuthController(IUserBUS userBUS)
        {
            _userBUS = userBUS;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            var result = await _userBUS.LoginAsync(loginDto);

            if (result == null)
            {
                return Unauthorized(new { message = "Sai tài khoản, mật khẩu hoặc vai trò không hợp lệ." });
            }

            return Ok(result);
        }
    }
}
