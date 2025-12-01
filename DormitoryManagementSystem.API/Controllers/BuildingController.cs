using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Buildings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingBUS _buildingBUS;

        public BuildingController(IBuildingBUS buildingBUS)
        {
            _buildingBUS = buildingBUS;
        }



        [HttpGet("buildings/lookup/combobox")] 
        
        public async Task<IActionResult> GetBuildingLookup()
        {
            try
            {
                var result = await _buildingBUS.GetBuildingLookupAsync();
                return Ok(result);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPost("buildings")]
        public async Task<IActionResult> CreateBuilding([FromBody] BuildingCreateDTO dto)
        {
            try
            {
                var result = await _buildingBUS.AddBuildingAsync(dto);
                return Ok(result);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPut("buildings/{id}")]
        public async Task<IActionResult> UpdateBuilding(string id, [FromBody] BuildingUpdateDTO dto)
        {

            try
            {
                await _buildingBUS.UpdateBuildingAsync(id, dto);
                return Ok(new { message = "Cập nhật thông tin tòa nhà thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpDelete("buildings/{id}")]
        public async Task<IActionResult> DeleteBuilding(string id)
        {
            try
            {
                await _buildingBUS.DeleteBuildingAsync(id);
                return Ok(new { message = "Xóa tòa nhà thành công!" });
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
