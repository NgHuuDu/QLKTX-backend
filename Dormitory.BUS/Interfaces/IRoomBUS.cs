using Dormitory.Models.Entities;

namespace Dormitory.BUS.Interfaces
{
    public interface IRoomBUS
    {
        public Task<IEnumerable<Room>> GetAllRoomsAsync();
        public Task<Room?> GetRoomByIDAsync(string id);
        public Task AddRoomAsync(Room room);
        public Task UpdateRoomAsync(Room room);
        public Task RemoveRoomAsync(string id);
    }
}
