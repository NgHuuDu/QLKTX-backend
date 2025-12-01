using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Violations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ViolationController : ControllerBase
    {
        private readonly IViolationBUS _violationBUS;

        public ViolationController(IViolationBUS violationBUS)
        {
            _violationBUS = violationBUS;
        }


        [HttpGet("student/violations")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyViolations([FromQuery] string? status)
        {
            try
            {
                 var studentId = User.FindFirst("StudentID")?.Value;

                if (string.IsNullOrEmpty(studentId))
                    return Unauthorized(new { message = "Không tìm thấy thông tin sinh viên." });

                // Logic xử lý tham số lọc
                if (string.IsNullOrEmpty(status) || status.ToLower() == "all")
                {
                    status = null; // Gán về null để BUS/DAO hiểu là "Lấy hết"
                }

            
                var violations = await _violationBUS.GetViolationsWithFilterAsync(status, studentId);

                return Ok(violations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }





        // ---------------------------------------------------------
        // KHU VỰC API CHO ADMIN
        // ---------------------------------------------------------




        //  Lấy danh sách vi phạm (Có lọc)
        [HttpGet("admin/violations")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAdminList(
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] string? roomId)
        {
            var list = await _violationBUS.GetViolationsForAdminAsync(search, status, roomId);
            return Ok(list);
        }

        // Thêm vi phạm mới 
        // POST: api/violation
        [HttpPost("admin/violations")]
       // [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateViolation([FromBody] ViolationCreateDTO dto)
        {
            try
            {
                var userId = User.FindFirst("UserID")?.Value;
                if (!string.IsNullOrEmpty(userId)) dto.ReportedByUserID = userId;

                var id = await _violationBUS.AddViolationAsync(dto);
                return Ok(new { message = "Tạo vi phạm thành công", id });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // Cập nhật vi phạm
        [HttpPut("admin/violations/{id}")]
       // [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateViolation(string id, [FromBody] ViolationUpdateDTO dto)
        {
            try
            {
                await _violationBUS.UpdateViolationAsync(id, dto);
                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        //  Xóa vi phạm
        [HttpDelete("admin/violations/{id}")]
       // [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteViolation(string id)
        {
            await _violationBUS.DeleteViolationAsync(id);
            return Ok(new { message = "Xóa thành công" });
        }

    }
}