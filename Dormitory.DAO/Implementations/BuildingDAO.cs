using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class BuildingDAO : IBuildingDAO
    {
        private readonly DormitoryContext _context;

        public BuildingDAO(DormitoryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Building>> GetAllAsync()
        {
            return await this._context.Buildings.ToListAsync();
        }

        public async Task<Building?> GetByIDAsync(string id)
        {
            return await this._context.Buildings.FindAsync(id);
        }

        public async Task AddBuildingAsync(Building building)
        {
            await this._context.Buildings.AddAsync(building);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveBuildingAsync(string id)
        {
            Building? b = await this._context.Buildings.FindAsync(id);

            if (b == null) return;
            else this._context.Buildings.Remove(b);

            await this._context.SaveChangesAsync();
        }

        public async Task UpdateBuildingAsync(Building building)
        {
            this._context.Buildings.Update(building);
            await this._context.SaveChangesAsync();
        }
    }
}
