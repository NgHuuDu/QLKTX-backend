using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Admins
{
    public class AdminDeleteDTO
    {
        [Required(ErrorMessage = "Mã quản trị viên là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã quản trị viên không được quá 10 ký tự")]
        public string AdminID { get; set; } = string.Empty;
    }
}