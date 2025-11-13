using Dormitory.BUS.Interfaces;
using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;

namespace Dormitory.BUS.Implementations
{
    public class RoomBUS : IRoomBUS
    {
        private readonly IRoomDAO roomDAO;

        public RoomBUS(IRoomDAO roomDAO)
        {
            this.roomDAO = roomDAO;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await this.roomDAO.GetAllAsync();
        }

        public async Task<Room?> GetRoomByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Room ID can not be null or empty.");

            return await this.roomDAO.GetByIDAsync(id);
        }

        public async Task AddRoomAsync(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));

            await this.roomDAO.AddRoomAsync(room);
        }

        public async Task UpdateRoomAsync(Room room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));

            Room? r = await this.roomDAO.GetByIDAsync(room.Roomid);
            if (r == null)
                throw new InvalidOperationException($"No room with id {room.Roomid} exist.");

            await this.roomDAO.UpdateRoomAsync(room);
        }

        public async Task RemoveRoomAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Room ID can not be null or empty.");

            Room? r = await this.roomDAO.GetByIDAsync(id);
            if (r == null)
                throw new InvalidOperationException($"No room with ID {id} exist.");

            await this.roomDAO.RemoveRoomAsync(id);
        }
    }
}
