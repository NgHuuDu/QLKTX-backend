using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Violations
{
    public class ViolationCreateDTO
    {
        [Required(ErrorMessage = "Mã vi phạm là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã vi phạm không được quá 10 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Mã vi phạm chỉ được chứa chữ và số")]
        public string ViolationID { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Mã sinh viên không được quá 10 ký tự")]
        public string? StudentID { get; set; }

        [Required(ErrorMessage = "Mã phòng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã phòng không được quá 10 ký tự")]
        public string RoomID { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Mã người báo cáo không được quá 10 ký tự")]
        public string? ReportedByUserID { get; set; }

        [Required(ErrorMessage = "Loại vi phạm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Loại vi phạm không được quá 100 ký tự")]
        public string ViolationType { get; set; } = string.Empty;

        public DateTime? ViolationDate { get; set; } = DateTime.Now;

        [Range(0, 1000000000, ErrorMessage = "Tiền phạt phải là số dương")]
        public decimal PenaltyFee { get; set; } = 0;

        [RegularExpression("^(Pending|Resolved|Paid)$", ErrorMessage = "Trạng thái phải là 'Pending', 'Resolved' hoặc 'Paid'")]
        public string Status { get; set; } = "Pending";
    }
}