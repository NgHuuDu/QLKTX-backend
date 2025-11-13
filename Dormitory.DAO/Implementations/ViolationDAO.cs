using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class ViolationDAO
    {
        private readonly DormitoryContext _context;

        public ViolationDAO(DormitoryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Violation>> GetAllViolationsAsync()
        {
            return await this._context.Violations.ToListAsync();
        }

        public async Task<Violation?> GetViolationByIDAsync(string id)
        {
            return await this._context.Violations.FindAsync(id);
        }

        public async Task AddNewViolationAsync(Violation violation)
        {
            await this._context.Violations.AddAsync(violation);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateViolation(Violation violation)
        {
            this._context.Violations.Update(violation);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveViolationAsync(string id)
        {
            Violation? v = await this._context.Violations.FindAsync(id);

            if (v == null) return;
            this._context.Violations.Remove(v);

            await this._context.SaveChangesAsync();
        }
    }
}
