using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.News
{
    public class NewsDeleteDTO
    {
        [Required(ErrorMessage = "Mã tin tức là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tin tức không được quá 10 ký tự")]
        public string NewsID { get; set; } = string.Empty;
    }
}