using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Students
{
    public class StudentCreateDTO
    {
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã sinh viên không được quá 10 ký tự")]
        public string StudentID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã tài khoản là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tài khoản không được quá 10 ký tự")]
        public string UserID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Ngành học không được quá 100 ký tự")]
        public string? Major { get; set; }

        [StringLength(50, ErrorMessage = "Khoa không được quá 50 ký tự")]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        [RegularExpression("^(Male|Female)$", ErrorMessage = "Giới tính phải là 'Male' hoặc 'Female'")]
        public string Gender { get; set; } = string.Empty;

        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Số CCCD là bắt buộc")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Số CCCD phải có đúng 12 ký tự")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số CCCD chỉ được chứa số")]
        public string CCCD { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ")]
        [StringLength(11, ErrorMessage = "Số điện thoại không được quá 11 ký tự")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        public string? Email { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ không được quá 255 ký tự")]
        public string? Address { get; set; }
    }
}