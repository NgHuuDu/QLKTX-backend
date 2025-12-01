using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Users
{
    public class UserUpdateDTO
    {
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string? Password { get; set; }

        [RegularExpression("^(Admin|Student)$", ErrorMessage = "Vai trò phải là 'Admin' hoặc 'Student'")]
        public string? Role { get; set; }

        public bool? IsActive { get; set; }
    }
}