namespace Dormitory.DTO.Buildings
{
    public class BuildingCreateDTO
    {
        public string BuildingName { get; set; } = string.Empty;
        public int NumberOfRooms { get; set; }
        public string Gender { get; set; } = string.Empty;
    }
}
