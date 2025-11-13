using Dormitory.Models.Entities;

namespace Dormitory.BUS.Interfaces
{
    public interface IRoleBUS
    {
        public Task<IEnumerable<Role>> GetAllRolesAsync();
        public Task<Role?> GetRoleByIDAsync(string id);
        public Task AddRoleAsync(Role role);
        public Task UpdateRoleAsync(Role role);
        public Task RemoveRoleAsync(string id);
    }
}
