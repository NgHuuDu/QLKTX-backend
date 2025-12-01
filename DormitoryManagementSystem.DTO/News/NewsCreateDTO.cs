using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.News
{
    public class NewsCreateDTO
    {
        [Required(ErrorMessage = "Mã tin tức là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã tin tức không được quá 10 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Mã tin tức chỉ được chứa chữ và số")]
        public string NewsID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tiêu đề không được quá 255 ký tự")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        [StringLength(10000, ErrorMessage = "Nội dung quá dài")]
        public string Content { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Danh mục không được quá 50 ký tự")]
        public string? Category { get; set; }

        [Range(0, 1, ErrorMessage = "Mức độ ưu tiên phải là 0 (Thường) hoặc 1 (Cao)")]
        public int Priority { get; set; } = 0;

        public bool IsVisible { get; set; } = true;

        [StringLength(10, ErrorMessage = "Mã người đăng không được quá 10 ký tự")]
        public string? AuthorID { get; set; }
    }
}