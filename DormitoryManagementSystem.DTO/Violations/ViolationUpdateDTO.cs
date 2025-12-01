using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Violations
{
    public class ViolationUpdateDTO
    {
        [Required(ErrorMessage = "Loại vi phạm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Loại vi phạm không được quá 100 ký tự")]
        public string ViolationType { get; set; } = string.Empty;

        [Range(0, 1000000000, ErrorMessage = "Tiền phạt phải là số dương")]
        public decimal PenaltyFee { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [RegularExpression("^(Pending|Resolved|Paid)$", ErrorMessage = "Trạng thái phải là 'Pending', 'Resolved' hoặc 'Paid'")]
        public string Status { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Mã sinh viên không được quá 10 ký tự")]
        public string? StudentID { get; set; }
    }
}