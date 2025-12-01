using DormitoryManagementSystem.BUS.Implementations;
using DormitoryManagementSystem.BUS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api/admin/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsBUS _statisticsBUS;

        public StatisticsController(IStatisticsBUS statisticsBUS)
        {
            _statisticsBUS = statisticsBUS;
        }
        
        
        
        //admin
        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = await _statisticsBUS.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        //Admin
        // API: Lấy doanh thu theo tháng (Vẽ biểu đồ cột/đường)
        [HttpGet("revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRevenueStats([FromQuery] int year)
        {
            try
            {
                // Nếu không truyền năm, mặc định lấy năm nay
                if (year == 0) year = DateTime.Now.Year;

                var stats = await _statisticsBUS.GetMonthlyRevenueAsync(year);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        // API: Lấy xu hướng lấp đầy (Vẽ biểu đồ đường Line Chart)
        // GET: api/statistics/occupancy-trend?year=2024
        [HttpGet("occupancy-trend")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOccupancyTrend([FromQuery] int year)
        {
            try
            {
                if (year == 0) year = DateTime.Now.Year;

                var stats = await _statisticsBUS.GetOccupancyTrendAsync(year);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // API: Thống kê tỷ lệ nam nữ
        // GET: api/statistics/gender
        [HttpGet("gender")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetGenderStats()
        {
            try
            {
                var stats = await _statisticsBUS.GetGenderStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // API: So sánh các tòa nhà (SV & Doanh thu)
        // GET: api/statistics/building-comparison?year=2024
        [HttpGet("building-comparison")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBuildingComparison([FromQuery] int? year)
        {
            try
            {
                var data = await _statisticsBUS.GetBuildingComparisonAsync(year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // API: Lấy xu hướng vi phạm (Vẽ biểu đồ đường/cột)
        // GET: api/statistics/violation-trend?year=2024
        [HttpGet("violation-trend")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetViolationTrend([FromQuery] int year)
        {
            try
            {
                if (year == 0) year = DateTime.Now.Year;

                var stats = await _statisticsBUS.GetViolationTrendAsync(year);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // API: Thống kê vi phạm (Chưa xử lý vs Đã xử lý)
        // GET: api/statistics/violation-summary
        [HttpGet("violation-summary")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetViolationSummaryStats()
        {
            try
            {
                var stats = await _statisticsBUS.GetViolationSummaryStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }


        //  Lấy thống kê thanh toán (Cho Payment Admin)
        // GET: api/payment/stats
        [HttpGet("payment-summary")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetPaymentStats()
        {
            try
            {
                var stats = await _statisticsBUS.GetPaymentStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
