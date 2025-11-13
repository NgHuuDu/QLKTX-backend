namespace Dormitory.DTO.Violations
{
    public class ViolationReadDTO
    {
        public string ViolationId { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string ViolationType { get; set; } = string.Empty;
        public DateOnly ViolationDate { get; set; } = new DateOnly();
        public decimal PenaltyFee { get; set; }
    }
}
