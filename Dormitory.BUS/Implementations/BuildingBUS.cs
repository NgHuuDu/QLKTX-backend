using Dormitory.BUS.Interfaces;
using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;

namespace Dormitory.BUS.Implementations
{
    public class BuildingBUS : IBuildingBUS
    {
        private readonly IBuildingDAO _buildingDAO;

        public BuildingBUS(IBuildingDAO BuildingDAO)
        {
            this._buildingDAO = BuildingDAO;
        }
        public async Task<IEnumerable<Building>> GetAllBuildingsAsync()
        {
            return await this._buildingDAO.GetAllAsync();
        }

        public async Task<Building?> GetBuildingByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Building ID can not be null or empty");

            return await this._buildingDAO.GetByIDAsync(id);
        }

        public async Task AddBuildingAsync(Building building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            await this._buildingDAO.AddBuildingAsync(building);
        }

        public async Task UpdateBuildingAsync(Building building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            Building? b = await this._buildingDAO.GetByIDAsync(building.Buildingid);
            if (b == null)
                throw new InvalidOperationException($"No building with id {building.Buildingid} exist.");

            await this._buildingDAO.UpdateBuildingAsync(building);
        }

        public async Task RemoveBuildingAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Building ID can not be null or empty");

            Building? b = await this._buildingDAO.GetByIDAsync(id);
            if (b == null)
                throw new InvalidOperationException($"No building with id {id} exist.");

            await this._buildingDAO.RemoveBuildingAsync(id);
        }
    }
}
