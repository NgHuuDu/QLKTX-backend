using Dormitory.Models.Entities;

namespace Dormitory.DAO.Interfaces
{
    public interface IRoomDAO
    {
        public Task<IEnumerable<Room>> GetAllAsync();
        public Task<Room?> GetByIDAsync(string id);
        public Task AddRoomAsync(Room room);
        public Task RemoveRoomAsync(string id);
        public Task UpdateRoomAsync(Room room);
    }
}
