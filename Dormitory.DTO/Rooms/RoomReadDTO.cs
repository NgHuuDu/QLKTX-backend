namespace Dormitory.DTO.Rooms
{
    public class RoomReadDTO
    {
        public string RoomID { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
        public string BuildingID { get; set; } = string.Empty;
    }
}
