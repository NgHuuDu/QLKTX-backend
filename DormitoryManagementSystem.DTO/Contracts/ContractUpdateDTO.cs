using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Contracts
{
    public class ContractUpdateDTO
    {
        [Required(ErrorMessage = "Mã phòng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã phòng không được quá 10 ký tự")]
        public string RoomID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateOnly StartTime { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateOnly EndTime { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [RegularExpression("^(Active|Expired|Terminated)$", ErrorMessage = "Trạng thái phải là 'Active', 'Expired' hoặc 'Terminated'")]
        public string Status { get; set; } = string.Empty;
    }
}