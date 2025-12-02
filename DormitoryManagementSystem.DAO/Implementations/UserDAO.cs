
using DormitoryManagementSystem.DAO.Context;
using DormitoryManagementSystem.DAO.Interfaces;
using DormitoryManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.DAO.Implementations
{
    public class UserDAO : IUserDAO
    {
        private readonly PostgreDbContext _context;

        public UserDAO(PostgreDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking()
                                       .Where(x => x.IsActive)
                                       .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersIncludingInactivesAsync()
        {
            return await _context.Users.AsNoTracking()
                                       .ToListAsync();
        }

        public async Task<User?> GetUserByIDAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.AsNoTracking()
                                       .Where(user => user.Username == username)
                                       .Include(user => user.Student)
                                       .FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string id)
        {
            User? u = await _context.Users.FindAsync(id);
            if (u == null) return;

            u.IsActive = false;

            _context.Update(u);
            await _context.SaveChangesAsync();
        }
    }
}
