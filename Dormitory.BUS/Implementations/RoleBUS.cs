using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Dormitory.BUS.Interfaces;


namespace Dormitory.BUS.Implementations
{
    public class RoleBUS : IRoleBUS
    {
        private readonly IRoleDAO roleDAO;
        public RoleBUS(IRoleDAO roleDAO)
        {
            this.roleDAO = roleDAO;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await this.roleDAO.GetAllRolesAsync();
        }

        public async Task<Role?> GetRoleByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Role ID can not be null or empty.");

            return await this.roleDAO.GetRoleByIDAsync(id);
        }

        public async Task AddRoleAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            await this.roleDAO.AddRoleAsync(role);
        }

        public async Task UpdateRoleAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            Role? r = await this.GetRoleByIDAsync(role.Roleid);
            if (r == null)
                throw new InvalidOperationException($"No role with id {role.Roleid} exist.");

            await this.roleDAO.UpdateRoleAsync(role);
        }

        public async Task RemoveRoleAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Role ID can not be null or empty.");

            Role? r = await this.GetRoleByIDAsync(id);
            if (r == null)
                throw new InvalidOperationException($"No role with id {id} exist.");

            await this.roleDAO.RemoveRoleAsync(id);
        }
    }
}
