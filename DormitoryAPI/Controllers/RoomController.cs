using Dormitory.BUS.Interfaces;
using Dormitory.DTO.Rooms;
using Dormitory.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomBUS _roomBUS;

        public RoomController(IRoomBUS roomBUS)
        {
            this._roomBUS = roomBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            IEnumerable<Room> rs = await this._roomBUS.GetAllRoomsAsync();
            return Ok(rs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomByID(string id)
        {
            try
            {
                Room? b = await this._roomBUS.GetRoomByIDAsync(id);
                if (b == null)
                    return NotFound(new { message = $"Room with id {id} not found." });

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
        public async Task<IActionResult> AddNewRoom([FromBody] RoomCreateDTO dto)
        {
            try
            {
                Room r = new Room
                {
                    Roomnumber = dto.RoomNumber,
                    Capacity = dto.Capacity,
                    Buildingid = dto.BuildingID
                };

                await this._roomBUS.AddRoomAsync(r);
                return CreatedAtAction(nameof(GetRoomByID), new { id = r.Roomid }, r);
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
        public async Task<IActionResult> UpdateRoom(string id, [FromBody] RoomUpdateDTO dto)
        {
            try
            {
                Room r = new Room
                {
                    Roomid = id,
                    Roomnumber = dto.RoomNumber,
                    Capacity = dto.Capacity,
                    Buildingid = dto.BuildingID
                };

                await this._roomBUS.UpdateRoomAsync(r);
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
        public async Task<IActionResult> RemoveRoom(string id)
        {
            try
            {
                await this._roomBUS.RemoveRoomAsync(id);
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
