using Dormitory.Models.Entities;

namespace Dormitory.DAO.Interfaces
{
    public interface IRoleDAO
    {
        public Task<IEnumerable<Role>> GetAllRolesAsync();
        public Task<Role?> GetRoleByIDAsync(string id);
        public Task AddRoleAsync(Role role);
        public Task UpdateRoleAsync(Role role);
        public Task RemoveRoleAsync(string id);
    }
}
