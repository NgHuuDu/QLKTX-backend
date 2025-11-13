namespace Dormitory.DTO.Rooms
{
    public class RoomCreateDTO
    {
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string BuildingID { get; set; } = string.Empty;
    }
}
