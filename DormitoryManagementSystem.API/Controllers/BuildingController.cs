using DormitoryManagementSystem.BUS.Interfaces;
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
    }
}
