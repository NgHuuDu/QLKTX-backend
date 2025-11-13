using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class StudentDAO : IStudentDAO
    {
        private readonly DormitoryContext _context;

        public StudentDAO(DormitoryContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await this._context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIDAsync(string id)
        {
            return await this._context.Students.FindAsync(id);
        }

        public async Task AddStudentAsync(Student student)
        {
            await this._context.Students.AddAsync(student);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            this._context.Students.Update(student);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveStudentAsync(string id)
        {
            Student? s = await this._context.Students.FindAsync(id);

            if (s == null) return;
            else this._context.Students.Remove(s);

            await this._context.SaveChangesAsync();
        }
    }
}
