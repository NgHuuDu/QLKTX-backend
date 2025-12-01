using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Users
{
    public class UserDeleteDTO
    {
        [Required(ErrorMessage = "Mã tài khoản là bắt buộc")]
        public string UserID { get; set; } = string.Empty;
    }
}