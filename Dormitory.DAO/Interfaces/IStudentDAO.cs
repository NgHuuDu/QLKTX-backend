using Dormitory.Models.Entities;

namespace Dormitory.DAO.Interfaces
{
    public interface IStudentDAO
    {
        public Task<IEnumerable<Student>> GetAllStudentsAsync();
        public Task<Student?> GetStudentByIDAsync(string id);
        public Task AddStudentAsync(Student student);
        public Task UpdateStudentAsync(Student student);
        public Task RemoveStudentAsync(string id);
    }
}
