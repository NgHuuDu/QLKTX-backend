using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class RoleDAO : IRoleDAO
    {
        private readonly DormitoryContext _context;

        public RoleDAO(DormitoryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await this._context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIDAsync(string id)
        {
            return await this._context.Roles.FindAsync(id);
        }

        public async Task AddRoleAsync(Role role)
        {
            await this._context.Roles.AddAsync(role);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            this._context.Roles.Update(role);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveRoleAsync(string id)
        {
            Role? r = await this.GetRoleByIDAsync(id);

            if (r == null) return;
            this._context.Roles.Remove(r);

            await this._context.SaveChangesAsync();
        }
    }
}
