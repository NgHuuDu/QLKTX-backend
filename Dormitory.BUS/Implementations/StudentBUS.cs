using Dormitory.BUS.Interfaces;
using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;

namespace Dormitory.BUS.Implementations
{
    public class StudentBUS : IStudentBUS
    {
        private readonly IStudentDAO studentDAO;

        public StudentBUS(IStudentDAO studentDAO)
        {
            this.studentDAO = studentDAO;
        }
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await this.studentDAO.GetAllStudentsAsync();
        }

        public async Task<Student?> GetStudentByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Student ID can not be null or empty.");

            return await this.studentDAO.GetStudentByIDAsync(id);
        }

        public async Task AddStudentAsync(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            Student? s = await this.GetStudentByIDAsync(student.Studentid);
            if (s != null)
                throw new InvalidOperationException($"A Student with id {student.Studentid} already exist.");

            await this.studentDAO.AddStudentAsync(student);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            Student? s = await this.GetStudentByIDAsync(student.Studentid);
            if (s == null)
                throw new InvalidOperationException($"No Student with id {student.Studentid} already exist.");
        }

        public async Task RemoveStudentAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Student ID can not be null or empty.");

            Student? s = await this.GetStudentByIDAsync(id);
            if (s != null)
                throw new InvalidOperationException($"A Student with id {id} already exist.");

            await this.studentDAO.RemoveStudentAsync(id);
        }
    }
}
