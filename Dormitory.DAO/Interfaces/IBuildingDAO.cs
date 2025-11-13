using Dormitory.Models.Entities;

namespace Dormitory.DAO.Interfaces
{
    public interface IBuildingDAO
    {
        Task<IEnumerable<Building>> GetAllAsync(); // GET
        Task<Building?> GetByIDAsync(string id); // GET
        Task AddBuildingAsync(Building building); // POST
        Task UpdateBuildingAsync(Building building); // PUT
        Task RemoveBuildingAsync(string id); // DELETE
    }
}
