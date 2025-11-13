using Dormitory.Models.Entities;

namespace Dormitory.BUS.Interfaces
{
    public interface IContractBUS
    {
        public Task<IEnumerable<Contract>> GetAllContractsAsync();
        public Task<Contract?> GetContractByIDAsync(string id);
        public Task AddContractAsync(Contract contract);
        public Task UpdateContractAsync(Contract contract);
        public Task RemoveContractAsync(string id);
    }
}
