using Dormitory.Models.Entities;

namespace Dormitory.DAO.Interfaces
{
    public interface IViolationDAO
    {
        public Task<IEnumerable<Violation>> GetAllViolationsAsync();
        public Task<Violation?> GetViolationByIdAsync(string id);
        public Task AddNewViolationAsync(Violation violation);
        public Task UpdateViolationAsync(Violation violation);
        public Task RemoveViolationAsync(string id);
    }
}
