namespace Dormitory.DTO.Paymenrs
{
    public class PaymentReadDTO
    {
        public string PaymentID { get; set; } = string.Empty;
        public string ContractID { get; set; } = string.Empty;
        public DateOnly PaymentDate { get; set; } = new DateOnly();
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
