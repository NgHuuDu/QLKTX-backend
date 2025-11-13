using Dormitory.Models.Entities;

namespace Dormitory.BUS.Interfaces
{
    public interface IBuildingBUS
    {
        public Task<IEnumerable<Building>> GetAllBuildingsAsync();
        public Task<Building?> GetBuildingByIDAsync(string id);
        public Task AddBuildingAsync(Building building);
        public Task UpdateBuildingAsync(Building building);
        public Task RemoveBuildingAsync(string id);
    }
}
