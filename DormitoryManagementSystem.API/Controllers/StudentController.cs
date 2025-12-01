using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentBUS _studentBUS;

        public StudentController(IStudentBUS studentBUS)
        {
            _studentBUS = studentBUS;
        }


        //Student 
        // API : lấy thông tin sinh viên
        [HttpGet("profile")]
        [Authorize(Roles = "Student")]// tắt này để test
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var studentId = User.FindFirst("StudentID")?.Value;
                if (string.IsNullOrEmpty(studentId))
                {
                    return Unauthorized(new { message = "Token không hợp lệ: Không tìm thấy StudentID." });
                }

                var profile = await _studentBUS.GetStudentProfileAsync(studentId);

                if (profile == null)
                    return NotFound(new { message = "Không tìm thấy hồ sơ sinh viên trong hệ thống." });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }


        
        [HttpPut("contact-info")]
        [Authorize(Roles = "Student")] 
        public async Task<IActionResult> UpdateContactInfo([FromBody] StudentContactUpdateDTO dto)
        {
            try
            {
                var studentId = User.FindFirst("StudentID")?.Value;


                if (string.IsNullOrEmpty(studentId))
                {
                    return Unauthorized(new { message = "Không xác định được sinh viên (Token lỗi)." });
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _studentBUS.UpdateContactInfoAsync(studentId, dto);

                return Ok(new { message = "Cập nhật thông tin liên lạc thành công!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
