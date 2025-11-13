using Dormitory.Models.Entities;

namespace Dormitory.BUS.Interfaces
{
    public interface IUserBUS
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User?> GetUserByIDAsync(string id);
        public Task AddUserAsync(User user);
        public Task UpdateUserAsync(User user);
        public Task RemoveUserAsync(string id);
    }
}
