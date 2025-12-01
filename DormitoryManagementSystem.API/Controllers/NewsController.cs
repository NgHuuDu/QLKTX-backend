using DormitoryManagementSystem.BUS.Interfaces;
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
}
