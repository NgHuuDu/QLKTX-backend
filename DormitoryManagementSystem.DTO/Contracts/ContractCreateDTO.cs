using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Contracts
{
    public class ContractCreateDTO
    {
        [Required(ErrorMessage = "Mã hợp đồng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã hợp đồng không được quá 10 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Mã hợp đồng chỉ được chứa chữ và số")]
        public string ContractID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã sinh viên không được quá 10 ký tự")]
        public string StudentID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã phòng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã phòng không được quá 10 ký tự")]
        public string RoomID { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Mã người tạo không được quá 10 ký tự")]
        public string? StaffUserID { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateOnly StartTime { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateOnly EndTime { get; set; }

        [RegularExpression("^(Active|Expired|Terminated)$", ErrorMessage = "Trạng thái phải là 'Active', 'Expired' hoặc 'Terminated'")]
        public string Status { get; set; } = "Active";
    }
}