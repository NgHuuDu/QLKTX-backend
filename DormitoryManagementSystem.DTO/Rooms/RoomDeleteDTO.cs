using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Rooms
{
    public class RoomDeleteDTO
    {
        [Required(ErrorMessage = "Mã phòng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã phòng không được quá 10 ký tự")]
        public string RoomID { get; set; } = string.Empty;
    }
}