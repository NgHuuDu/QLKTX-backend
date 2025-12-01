using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Violations
{
    public class ViolationDeleteDTO
    {
        [Required(ErrorMessage = "Mã vi phạm là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã vi phạm không được quá 10 ký tự")]
        public string ViolationID { get; set; } = string.Empty;
    }
}