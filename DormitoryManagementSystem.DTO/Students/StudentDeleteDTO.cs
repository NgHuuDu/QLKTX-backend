using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Students
{
    public class StudentDeleteDTO
    {
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã sinh viên không được quá 10 ký tự")]
        public string StudentID { get; set; } = string.Empty;
    }
}
