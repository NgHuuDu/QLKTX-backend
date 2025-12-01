using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Buildings
{
    public class BuildingDeleteDTO
    {
        [Required(ErrorMessage = "Mã tòa nhà là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tòa nhà không được quá 10 ký tự")]
        public string BuildingID { get; set; } = string.Empty;
    }
}
