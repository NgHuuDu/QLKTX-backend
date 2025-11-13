namespace Dormitory.DTO.Rooms
{
    public class RoomUpdateDTO
    {
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string BuildingID { get; set; } = string.Empty;
    }
}
