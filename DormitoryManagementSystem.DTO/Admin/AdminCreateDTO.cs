using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Admins
{
    public class AdminCreateDTO
    {
        [Required(ErrorMessage = "Mã quản trị viên là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã quản trị viên không được quá 10 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Mã quản trị viên chỉ được chứa chữ cái, số và dấu gạch dưới")]
        public string AdminID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã tài khoản (UserID) là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tài khoản không được quá 10 ký tự")]
        public string UserID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số CCCD là bắt buộc")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Số CCCD phải có đúng 12 ký tự")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số CCCD chỉ được chứa số")]
        public string IDcard { get; set; } = string.Empty;

        [RegularExpression("^(Male|Female)$", ErrorMessage = "Giới tính phải là 'Male' (Nam) hoặc 'Female' (Nữ)")]
        public string? Gender { get; set; }

        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được quá 15 ký tự")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        public string? Email { get; set; }
    }
}