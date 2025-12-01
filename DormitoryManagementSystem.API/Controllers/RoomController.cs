using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Rooms;
using DormitoryManagementSystem.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.NetworkInformation;

[Route("api")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomBUS _roomBUS;

    public RoomController(IRoomBUS roomBUS)
    {
        _roomBUS = roomBUS;
    }



    // ---------------------------------------------------------
    // KHU VỰC API CHO STUDENT
    // ---------------------------------------------------------


    [HttpGet("student/rooms/{RoomID}")]
    [Authorize(Roles = "Student")]// Tắt cái này mới test được, mới lên khi chạy chính thức
    public async Task<IActionResult> GetRoomDetail(string RoomID)
    {
        try
        {
            var roomDetail = await _roomBUS.GetRoomDetailByIDAsync(RoomID);

            if (roomDetail == null)
            {
                return NotFound(new { message = $"Không tìm thấy phòng với ID: {RoomID}" });
            }

            return Ok(roomDetail);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }


    //Student 
    // Duyệt phòng ở FE Student, cái trang có hiện thị phòng chi tiết á
    [HttpGet("student/rooms/cards")]
     [Authorize(Roles = "Student")] // Tắt cái này mới test được, mới lên khi chạy chính thức
    public async Task<IActionResult> SearchRoomInCard(
    [FromQuery] string? buildingId,
    [FromQuery] int? roomNumber,
    [FromQuery] int? capacity,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice,
    [FromQuery] bool? allowCooking,    
    [FromQuery] bool? airConditioner)  
    {
        try
        {
            var rooms = await _roomBUS.SearchRoomInCardAsync(
                buildingId, roomNumber, capacity, minPrice,maxPrice, allowCooking, airConditioner);

            if (rooms == null || !rooms.Any())
            {
                return Ok(new List<RoomDetailDTO>());
            }

            return Ok(rooms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    // Duyệt phòng ở FE Student, cái trang có hiện thị lưới á

    [HttpGet("student/rooms/grid")]
    [Authorize(Roles = "Student")] // Tắt cái này mới test được, mới lên khi chạy chính thức
    public async Task<IActionResult> SearchRoomInGrid(
    [FromQuery] string? buildingId,
    [FromQuery] int? roomNumber,
    [FromQuery] int? capacity,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice


    )
    {
        try
        {
            var rooms = await _roomBUS.SearchRoomInGridAsync(
                buildingId, roomNumber, capacity, minPrice,maxPrice);

            if (rooms == null || !rooms.Any())
            {
                return Ok(new List<RoomGridDTO>());
            }

            return Ok(rooms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }





    // ---------------------------------------------------------
    // KHU VỰC API CHUNG CHO ADMIN VÀ STUDENT
    // ---------------------------------------------------------


    [HttpGet("rooms/capacities")]
    public async Task<IActionResult> GetCapacities()
    {
        try
        {
            var result = await _roomBUS.GetRoomCapacitiesAsync();
            return Ok(result); 
        }
        catch (Exception ex) { return StatusCode(500, ex.Message); }
    }

    // URL: api/room/price-ranges
    [HttpGet("rooms/price-ranges")]
    public IActionResult GetPriceRanges()
    {
        var result = _roomBUS.GetPriceRanges();
        return Ok(result);
    }

    // ---------------------------------------------------------
    // KHU VỰC API CHUNG CHO ADMIN 
    // ---------------------------------------------------------

    // Tạo phòng mới admin
    // frmAddRoom
    [HttpPost("admin/rooms")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRoom([FromBody] RoomCreateDTO dto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _roomBUS.AddRoomAsync(dto);
            return Ok(new { message = "Thêm phòng thành công!"});
        }
        catch(Exception ex) 
        {
            return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }
    // frmRoomDetail
    // Cap nhat phong
    [HttpPut("admin/rooms/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoom(string id, [FromBody] RoomUpdateDTO dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Dữ liệu phòng là bắt buộc" });
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _roomBUS.UpdateRoomAsync(id, dto);
            return Ok(new { message = "Cập nhật thông tin phòng thành công!" });
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

    // Xoa phong

    [HttpDelete("admin/rooms/{id}")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> DeleteRoom(string id)
    {
        try
        {
            await _roomBUS.DeleteRoomAsync(id);
            return Ok(new { message = "Xóa phòng thành công" });
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

    // Lọc theo tên và mã phòng
    //ucRoomManagement
    [HttpGet("admin/rooms/search")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> SearchRooms([FromQuery] string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return Ok(new List<RoomReadDTO>());
            }

            var result = await _roomBUS.SearchRoomsAsync(q);

            if (result == null || !result.Any())
            {
                return NotFound(new { message = $"Không tìm thấy phòng nào với từ khóa '{q}'" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }
}
