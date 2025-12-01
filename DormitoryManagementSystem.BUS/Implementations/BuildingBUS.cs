using AutoMapper;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DAO.Interfaces;
using DormitoryManagementSystem.DTO.Buildings;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.BUS.Implementations
{
    public class BuildingBUS : IBuildingBUS
    {
        private readonly IBuildingDAO _dao;
        private readonly IRoomDAO _daoRoom;
        private readonly IMapper _mapper;

        public BuildingBUS(IBuildingDAO dao, IRoomDAO daoRoom, IMapper mapper)
        {
            this._dao = dao;
            this._daoRoom = daoRoom;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<BuildingReadDTO>> GetAllBuildingAsync()
        {
            IEnumerable<Building> buildings = await this._dao.GetAllBuildingAsync();
            return this._mapper.Map<IEnumerable<BuildingReadDTO>>(buildings);
        }

        public async Task<IEnumerable<BuildingReadDTO>> GetAllBuildingIncludingInactivesAsync()
        {
            IEnumerable<Building> buildings = await this._dao.GetAllBuildingIncludingInactivesAsync();
            return this._mapper.Map<IEnumerable<BuildingReadDTO>>(buildings);
        }

        public async Task<BuildingReadDTO?> GetByIDAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Building ID không thể để trống");

            Building? building = await this._dao.GetByIDAsync(id);
            if (building == null) return null;

            return this._mapper.Map<BuildingReadDTO>(building);
        }

        public async Task<string> AddBuildingAsync(BuildingCreateDTO dto)
        {
            Building? existing = await this._dao.GetByIDAsync(dto.BuildingID);
            if (existing != null)
                throw new InvalidOperationException($"Không có tòa nhà với ID {dto.BuildingID}.");

            Building building = this._mapper.Map<Building>(dto);
            await this._dao.AddBuildingAsync(building);

            return building.Buildingid;
        }

        public async Task UpdateBuildingAsync(string id, BuildingUpdateDTO dto)
        {
            Building? building = await this._dao.GetByIDAsync(id);
            if (building == null)
                throw new KeyNotFoundException($"Không có tòa nhà với ID {id}.");

            if (building.Currentoccupancy > 0 && building.Gendertype != dto.Gender)
            {
                throw new InvalidOperationException($"Không thể thay đổi giới tính thành '{dto.Gender}' " +
                                                    $"bởi vì có {building.Currentoccupancy} sinh viên đang sống ở đây.");
            }

            IEnumerable<Room> actualRooms = await this._daoRoom.GetRoomsByBuildingIDAsync(id);
            if (dto.NumberOfRooms < actualRooms.Count())
            {
                throw new InvalidOperationException($"Không thể đặt số phòng thành {dto.NumberOfRooms} " +
                                                    $"bởi vì hiện tại có {actualRooms.Count()} phòng trong hệ thống.");
            }

            // Map dữ liệu từ dto -> đè lên building hiện tại
            this._mapper.Map(dto, building);
            building.Buildingid = id;

            await this._dao.UpdateBuildingAsync(building);
        }

        public async Task DeleteBuildingAsync(string id)
        {
            Building? building = await _dao.GetByIDAsync(id);
            if (building == null)
                throw new KeyNotFoundException($"Không có tòa nhà với ID {id}.");

            if (building.Currentoccupancy > 0)
                throw new InvalidOperationException($"Không thể xóa tòa nhà {id} vì có sinh viên đang sống ở đó.");

            // CASCADE SOFT DELETE: Set toàn bộ phòng thành 'Inactive'
            IEnumerable<Room> rooms = await _daoRoom.GetRoomsByBuildingIDAsync(id);

            foreach (Room room in rooms)
                // SOFT DELETE EACH ROOM
                await _daoRoom.DeleteRoomAsync(room.Roomid);

            //SOFT DELETE BUILDING
            await _dao.DeleteBuildingAsync(id);
        }




        // Mới thêm - Lấy danh sách phòng để hiện trên comboBox Student
        public async Task<IEnumerable<BuildingLookupDTO>> GetBuildingLookupAsync()
        {
            var buildings = await _dao.GetAllBuildingAsync();

            return buildings.Select(b => new BuildingLookupDTO
            {
                BuildingID = b.Buildingid,
                BuildingName = b.Buildingname
            });
        }
    }
}