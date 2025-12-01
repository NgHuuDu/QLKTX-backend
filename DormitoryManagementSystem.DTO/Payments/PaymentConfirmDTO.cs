using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.DTO.Payments
{
    public class PaymentConfirmDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [RegularExpression("^(Cash|Bank Transfer)$", ErrorMessage = "Phương thức không hợp lệ")]
        public string PaymentMethod { get; set; } = "Cash";

        public string? Note { get; set; }
    }
}
