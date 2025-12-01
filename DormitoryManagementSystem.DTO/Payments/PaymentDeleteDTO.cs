using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Payments
{
    public class PaymentDeleteDTO
    {
        [Required(ErrorMessage = "Mã thanh toán là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã thanh toán không được quá 10 ký tự")]
        public string PaymentID { get; set; } = string.Empty;
    }
}
