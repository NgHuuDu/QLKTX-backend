using Dormitory.Models.Entities;

namespace Dormitory.BUS.Interfaces
{
    public interface IViolationBUS
    {
        public Task<IEnumerable<Violation>> GetAllViolationsAsync();
        public Task<Violation?> GetViolationByIdAsync(string id);
        public Task AddNewViolationAsync(Violation violation);
        public Task UpdateViolationAsync(Violation violation);
        public Task RemoveViolationAsync(string id);
    }
}
