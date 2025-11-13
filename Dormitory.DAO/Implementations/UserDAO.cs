using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class UserDAO : IUserDAO
    {
        private readonly DormitoryContext _context;

        public UserDAO(DormitoryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await this._context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIDAsync(string id)
        {
            return await this._context.Users.FindAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(string id)
        {
            User? u = await this.GetUserByIDAsync(id);

            if (u == null) return;
            this._context.Users.Remove(u);

            await this._context.SaveChangesAsync();
        }
    }
}
