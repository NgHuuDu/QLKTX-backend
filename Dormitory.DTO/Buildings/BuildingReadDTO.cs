namespace Dormitory.DTO.Buildings
{
    public class BuildingReadDTO
    {
        public string BuildingID { get; set; } = string.Empty;
        public string BuildingName { get; set; } = string.Empty;
        public int NumberOfRooms { get; set; }
        public int CurrentOccupancy { get; set; }
        public string Gender { get; set; } = string.Empty;
    }
}
