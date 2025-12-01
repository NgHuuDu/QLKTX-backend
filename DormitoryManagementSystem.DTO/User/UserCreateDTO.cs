using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Users
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Mã tài khoản là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tài khoản không được quá 10 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Mã tài khoản chỉ được chứa chữ, số và dấu gạch dưới")]
        public string UserID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [RegularExpression("^(Admin|Student)$", ErrorMessage = "Vai trò phải là 'Admin' hoặc 'Student'")]
        public string Role { get; set; } = string.Empty;
    }
}