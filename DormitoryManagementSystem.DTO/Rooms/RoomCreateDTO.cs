using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Rooms
{
    public class RoomCreateDTO
    {
        [Required(ErrorMessage = "Mã phòng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã phòng không được quá 10 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]*$", ErrorMessage = "Mã phòng chỉ được chứa chữ, số và ký tự đặc biệt cơ bản")]
        public string RoomID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số phòng là bắt buộc")]
        [Range(1, 9999, ErrorMessage = "Số phòng không hợp lệ")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Mã tòa nhà là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tòa nhà không được quá 10 ký tự")]
        public string BuildingID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sức chứa là bắt buộc")]
        [Range(1, 50, ErrorMessage = "Sức chứa phải từ 1 đến 50 người")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Giá phòng là bắt buộc")]
        [Range(0, 1000000000, ErrorMessage = "Giá phòng phải là số dương")]
        public decimal Price { get; set; }

        [RegularExpression("^(Active|Maintenance)$", ErrorMessage = "Trạng thái phải là 'Active' hoặc 'Maintenance'")]
        public string Status { get; set; } = "Active";

        public bool AllowCooking { get; set; } = false;
        public bool AirConditioner { get; set; } = false;
    }
}