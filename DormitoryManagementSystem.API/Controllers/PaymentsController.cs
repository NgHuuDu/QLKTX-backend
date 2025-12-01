using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Payments;
using DormitoryManagementSystem.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentBUS _paymentBUS;

    public PaymentController(IPaymentBUS paymentBUS)
    {
        _paymentBUS = paymentBUS;
    }


    // ---------------------------------------------------------
    // KHU VỰC API CHO STUDENT
    // ---------------------------------------------------------



    // API: Lấy danh sách thanh toán của sinh viên (Có thể lọc theo trạng thái)
    [HttpGet("student/payments")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyPayments([FromQuery] string? status)
    {
        var studentId = User.FindFirst("StudentID")?.Value; // Lấy từ Token
        var list = await _paymentBUS.GetPaymentsByStudentAndStatusAsync(studentId, status);

        return Ok(list);
    }

    



    // ---------------------------------------------------------
    // KHU VỰC API CHO ADMIN
    // ---------------------------------------------------------

    // Lấy danh sách & Lọc
    [HttpGet("admin/payments")]
   [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAdminPaymentList(
        [FromQuery] int? month,
        [FromQuery] string? status,
        [FromQuery] string? building,
        [FromQuery] string? search) // Gồm StudentName, StudentID
    {
        var list = await _paymentBUS.GetPaymentsForAdminAsync(month, status, building, search);
        return Ok(list);
    }
    /*
    //  Tạo hóa đơn mới
    [HttpPost("admin/payments")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBill([FromBody] PaymentCreateDTO dto)
    {
        try
        {
            var id = await _paymentBUS.AddPaymentAsync(dto);
            return Ok(new { id });
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }
    */
    // PUT: api/payment/{id}/confirm
    [HttpPut("admin/payments/{id}/confirm")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ConfirmPayment(string id, [FromBody] PaymentConfirmDTO dto)
    {
        try
        {
            // Validate dữ liệu (Check Cash/Bank Transfer)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _paymentBUS.ConfirmPaymentAsync(id, dto);

            return Ok(new { message = "Ghi nhận thanh toán thành công!" });
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

    //Xóa hóa đơn (Nếu tạo sai)
    [HttpDelete("admin/payments/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBill(string id)
    {
        await _paymentBUS.DeletePaymentAsync(id);
        return Ok(new { message = "Đã xóa hóa đơn" });
    }

    [HttpPost("admin/payments/auto-generate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GenerateBills([FromQuery] int month, [FromQuery] int year)
    {
        try
        {
            if (year == 0) year = DateTime.Now.Year;

            int count = await _paymentBUS.GenerateMonthlyBillsAsync(month, year);

            return Ok(new { message = $"Đã tạo thành công {count} hóa đơn tiền phòng cho tháng {month}/{year}!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
        }
    }


}