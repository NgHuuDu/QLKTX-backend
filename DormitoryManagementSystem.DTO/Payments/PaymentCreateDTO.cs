using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Payments
{
    public class PaymentCreateDTO
    {
        [Required(ErrorMessage = "Mã thanh toán là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã thanh toán không được quá 10 ký tự")]
        public string PaymentID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã hợp đồng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã hợp đồng không được quá 10 ký tự")]
        public string ContractID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tháng hóa đơn là bắt buộc")]
        [Range(1, 12, ErrorMessage = "Tháng phải từ 1 đến 12")]
        public int BillMonth { get; set; }

        [Required(ErrorMessage = "Số tiền cần đóng là bắt buộc")]
        [Range(0, 1000000000, ErrorMessage = "Số tiền phải là số dương")]
        public decimal PaymentAmount { get; set; }

        [Range(0, 1000000000, ErrorMessage = "Số tiền đã đóng phải là số dương")]
        public decimal PaidAmount { get; set; } = 0;

        [RegularExpression("^(Cash|Bank Transfer)$", ErrorMessage = "Phương thức phải là 'Cash', 'Bank Transfer'")]
        public string? PaymentMethod { get; set; }

        [RegularExpression("^(Unpaid|Paid|Late|Refunded)$", ErrorMessage = "Trạng thái thanh toán không hợp lệ")]
        public string PaymentStatus { get; set; } = "Unpaid";

        public DateTime? PaymentDate { get; set; } = DateTime.Now;

        [StringLength(255, ErrorMessage = "Mô tả không được quá 255 ký tự")]
        public string? Description { get; set; }
    }
}