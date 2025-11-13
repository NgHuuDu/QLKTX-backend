using Dormitory.BUS.Interfaces;
using Dormitory.Models.Entities;
using Dormitory.DTO.Buildings;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingBUS buildingBUS;

        public BuildingController(IBuildingBUS buildingBUS)
        {
            this.buildingBUS = buildingBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetBuildings()
        {
            IEnumerable<Building> bs = await this.buildingBUS.GetAllBuildingsAsync();
            return Ok(bs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuildingByID(string id)
        {
            try
            {
                Building? b = await this.buildingBUS.GetBuildingByIDAsync(id);
                if (b == null)
                    return NotFound(new { message = $"Building with id {id} not found." });
                return Ok(b);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBuilding([FromBody] BuildingCreateDTO dto)
        {
            try
            {
                Building b = new Building
                {
                    Buildingname = dto.BuildingName,
                    Numberofrooms = dto.NumberOfRooms,
                    Gender = dto.Gender
                };

                await this.buildingBUS.AddBuildingAsync(b);
                return CreatedAtAction(nameof(GetBuildingByID), new { id = b.Buildingid }, b);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBuilding(string id, [FromBody] BuildingUpdateDTO dto)
        {
            try
            {
                Building b = new Building
                {
                    Buildingid = id,
                    Buildingname = dto.BuildingName,
                    Numberofrooms = dto.NumberOfRooms,
                    Gender = dto.Gender
                };

                await this.buildingBUS.UpdateBuildingAsync(b);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBuilding(string id)
        {
            try
            {
                await this.buildingBUS.RemoveBuildingAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }
    }
}
