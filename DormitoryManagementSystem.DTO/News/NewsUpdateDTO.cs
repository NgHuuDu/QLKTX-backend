using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.News
{
    public class NewsUpdateDTO
    {
        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tiêu đề không được quá 255 ký tự")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        [StringLength(10000, ErrorMessage = "Nội dung quá dài")]
        public string Content { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Danh mục không được quá 50 ký tự")]
        public string? Category { get; set; }

        [Range(0, 1, ErrorMessage = "Mức độ ưu tiên phải là 0 (Thường) hoặc 1 (Cao)")]
        public int Priority { get; set; }

        public bool IsVisible { get; set; }
    }
}