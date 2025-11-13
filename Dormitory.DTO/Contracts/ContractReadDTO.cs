namespace Dormitory.DTO.Contracts
{
    public class ContractReadDTO
    {
        public string ContractId { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string RoomID { get; set; } = string.Empty;
        public DateOnly StartTime { get; set; } = new DateOnly();
        public DateOnly EndTime { get; set; } = new DateOnly();
    }
}
