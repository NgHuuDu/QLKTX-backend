namespace Dormitory.DTO.Students
{
    public class StudentCreateDTO
    {
        public string StudentName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Department {  get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; } = new DateOnly();
        public string PhoneNumber {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;
    }
}
