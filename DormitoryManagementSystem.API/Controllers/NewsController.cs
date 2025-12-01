using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/news")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly INewsBUS _newsBUS;

    public NewsController(INewsBUS newsBUS)
    {
        _newsBUS = newsBUS;
    }

    // Dùng để hiển thị danh sách tin tức (chỉ có tiêu đề, ảnh đại diện, ngày đăng) -> trong trang chủ á
    [HttpGet("summary")]
    public async Task<IActionResult> GetNewsList()
    {
        try
        {
            var news = await _newsBUS.GetNewsSummariesAsync();
            return Ok(news);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // Lấy chi tiết nội dung  -> Dùng khi click vào xem -> là nó sẽ lấy cái id của tin tức đó để lấy full content
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsByID(string id)
    {
        try
        {
            var news = await _newsBUS.GetNewsByIDAsync(id);

            if (news == null)
                return NotFound(new { message = "Tin tức không tồn tại" });

            return Ok(news);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateNews([FromBody] NewsCreateDTO dto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var newNewsId = await _newsBUS.AddNewsAsync(dto);
            return StatusCode(201, new { message = "Đăng tin thành công!", newsId = newNewsId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateNews(string id, [FromBody] NewsUpdateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _newsBUS.UpdateNewsAsync(id, dto);
            return Ok(new { message = "Cập nhật bài viết thành công!" });
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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteNews(string id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _newsBUS.DeleteNewsAsync(id);
            return Ok(new { message = "Xóa bài viết thành công!" });
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
}