using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;

namespace Dormitory.BUS.Implementations
{
    public class UserBUS
    {
        private readonly IUserDAO userDAO;
        public UserBUS(IUserDAO userDAO)
        {
            this.userDAO = userDAO;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await this.userDAO.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("User ID can not be null or empty.");

            return await this.userDAO.GetUserByIDAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            User? u = await this.GetUserByIDAsync(user.Userid);
            if (u != null)
                throw new InvalidOperationException($"A User with ID {user.Userid} already existed.");

            await this.userDAO.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            User? u = await this.GetUserByIDAsync(user.Userid);
            if (u == null)
                throw new InvalidOperationException($"No user with id {user.Userid} exist.");

            await this.userDAO.UpdateUserAsync(user);
        }

        public async Task RemoveUserAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("User ID can not be null or empty.");

            User? u = await this.GetUserByIDAsync(id);
            if (u == null)
                throw new InvalidOperationException($"No user with id {id} exist.");

            await this.userDAO.RemoveUserAsync(id);
        }
    }
}
