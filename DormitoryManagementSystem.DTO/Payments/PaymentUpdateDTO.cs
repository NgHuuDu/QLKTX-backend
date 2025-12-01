using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Payments
{
    public class PaymentUpdateDTO
    {
        [Range(0, 1000000000, ErrorMessage = "Số tiền đã đóng phải là số dương")]
        public decimal PaidAmount { get; set; }

        [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc khi cập nhật")]
        [RegularExpression("^(Cash|Bank Transfer)$", ErrorMessage = "Phương thức phải là 'Cash', 'Bank Transfer'")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [RegularExpression("^(Unpaid|Paid|Late|Refunded)$", ErrorMessage = "Trạng thái thanh toán không hợp lệ")]
        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [StringLength(255, ErrorMessage = "Mô tả không được quá 255 ký tự")]
        public string? Description { get; set; }
    }
}