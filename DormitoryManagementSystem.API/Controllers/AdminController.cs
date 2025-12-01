using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Admins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBUS _adminBUS;

        public AdminController(IAdminBUS adminBUS)
        {
            _adminBUS = adminBUS;
        }

        [HttpGet("admins")]

        public async Task<ActionResult<IEnumerable<AdminReadDTO>>> GetAllAdmins()
        {
            var admins = await _adminBUS.GetAllAdminsAsync();
            return Ok(admins.OrderBy(a => a.UserID).ToList());
        }
    }
}

