using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Contracts;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractBUS _contractBUS;

        public ContractController(IContractBUS contractBUS)
        {
            _contractBUS = contractBUS;
        }




        //Student
        // API: Sinh viên xem hợp đồng CỦA CHÍNH MÌNH
        [HttpGet("student/my-contracts")]
        [Authorize(Roles = "Student")] // Nhớ bật lại khi có Token
        public async Task<IActionResult> GetMyContracts()
        {
            try
            {
                var studentId = User.FindFirst("StudentID")?.Value;

                if (string.IsNullOrEmpty(studentId))
                    return Unauthorized(new { message = "Không tìm thấy thông tin sinh viên." });

                var contracts = await _contractBUS.GetContractFullDetailAsync(studentId);

                if (contracts == null) contracts = null;

                return Ok(contracts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //Student
        // API: Sinh viên gửi đơn đăng ký
        [HttpPost("student/contracts")]
        [Authorize(Roles = "Student")] 
        public async Task<IActionResult> RegisterContract([FromBody] ContractRegisterDTO dto)
        {
            try
            {
                 var studentId = User.FindFirst("StudentID")?.Value;
                if (string.IsNullOrEmpty(studentId))
                    return Unauthorized(new { message = "Không xác định được sinh viên." });

                var contractId = await _contractBUS.RegisterContractAsync(studentId, dto);

                return Ok(new { message = "Gửi đơn đăng ký thành công!", contractId = contractId });
            }
            catch (InvalidOperationException ex) // Lỗi phòng đầy,...
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }



        // Admin
        //ucContractManagement
        [HttpGet("admin/contracts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetContracts([FromQuery] string? searchTerm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _contractBUS.GetContractsAsync(searchTerm);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpGet("admin/contracts/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetContractById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var contract = await _contractBUS.GetContractByIDAsync(id);
                if (contract == null)
                {
                    return NotFound(new { message = "Không tìm thấy hợp đồng." });
                }
                return Ok(contract);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /*
        // Tao hop dong
        // frmAddContract
        [HttpPost("admin/contracts")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateContract([FromBody] ContractCreateDTO dto, string staffUserID)
        {

            try
            {
                if (string.IsNullOrEmpty(staffUserID))
                {
                    return BadRequest(new { message = "Không xác định được người tạo (StaffUserID). Vui lòng đăng nhập lại." });
                }
                string staffUserId = User.FindFirst("UserID")?.Value; // nguoi thuc hien tao lay tu luc dang nhap
                await _contractBUS.AddContractAsync(dto, staffUserID);
                return Ok(new { message = "Tạo hợp đồng thành công!" });
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
        */

        // frmContractDetail
        // Cap nhat phong
        [HttpPut("admin/contracts/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateContract(string id, [FromBody] ContractUpdateDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _contractBUS.UpdateContractAsync(id, dto);
                return Ok(new { message = "Cập nhật hợp đồng thành công!" });
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

        // Xoa hop dong
        [HttpDelete("admin/contracts/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteContract(string id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _contractBUS.DeleteContractAsync(id);
                return Ok(new { message = "Xóa hợp đồng thành công!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // frmFilterContract
        [HttpGet("admin/contracts/filter")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetContractsByMultiCondition([FromQuery] ContractFilterDTO filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _contractBUS.GetContractsByMultiConditionAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}