using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class RoomDAO : IRoomDAO
    {
        private DormitoryContext _context;

        public RoomDAO(DormitoryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await this._context.Rooms.ToListAsync();
        }

        public async Task<Room?> GetByIDAsync(string id)
        {
            return await this._context.Rooms.FindAsync(id);
        }

        public async Task AddRoomAsync(Room room)
        {
            await this._context.Rooms.AddAsync(room);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateRoomAsync(Room room)
        {
            this._context.Rooms.Update(room);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveRoomAsync(string id)
        {
            Room? r = await this._context.Rooms.FindAsync(id);

            if (r == null) return;
            else this._context.Rooms.Remove(r);

            await this._context.SaveChangesAsync();
        }
    }
}
