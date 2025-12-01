using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Buildings
{
    public class BuildingCreateDTO
    {
        [Required(ErrorMessage = "Mã tòa nhà là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tòa nhà không được quá 10 ký tự")]
        public string BuildingID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên tòa nhà là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên tòa nhà không được quá 50 ký tự")]
        public string BuildingName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số lượng phòng là bắt buộc")]
        [Range(1, 1000, ErrorMessage = "Số lượng phòng phải từ 1 đến 1000")]
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "Loại giới tính là bắt buộc")]
        [RegularExpression("^(Male|Female|Mixed)$", ErrorMessage = "Loại giới tính phải là 'Male', 'Female' hoặc 'Mixed'")]
        public string Gender { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}