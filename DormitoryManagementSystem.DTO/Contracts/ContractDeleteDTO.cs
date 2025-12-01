using System.ComponentModel.DataAnnotations;

namespace DormitoryManagementSystem.DTO.Contracts
{
    public class ContractDeleteDTO
    {
        [Required(ErrorMessage = "Mã hợp đồng là bắt buộc")]
        [StringLength(10, ErrorMessage = "Mã hợp đồng không được quá 10 ký tự")]
        public string ContractID { get; set; } = string.Empty;
    }
}