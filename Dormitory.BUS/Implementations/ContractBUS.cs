using Dormitory.BUS.Interfaces;
using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;

namespace Dormitory.BUS.Implementations
{
    public class ContractBUS : IContractBUS
    {
        private readonly IContractDAO contractDAO;

        public ContractBUS(IContractDAO contractDAO)
        {
            this.contractDAO = contractDAO;
        }

        public async Task<IEnumerable<Contract>> GetAllContractsAsync()
        {
            return await this.contractDAO.GetAllContractsAsync();
        }

        public async Task<Contract?> GetContractByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Contract ID can not be null or empty.");

            return await this.contractDAO.GetContractByIDAsync(id);
        }

        public async Task AddContractAsync(Contract contract)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            await this.contractDAO.AddContractAsync(contract);
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            Contract? c = await this.contractDAO.GetContractByIDAsync(contract.Contractid);
            if (c == null)
                throw new InvalidOperationException($"No contract with id {contract.Contractid} exist.");

            await this.contractDAO.UpdateContractAsync(contract);
        }

        public async Task RemoveContractAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Contract ID can not be null or empty.");

            Contract? c = await this.contractDAO.GetContractByIDAsync(id);
            if (c == null)
                throw new InvalidOperationException($"No contract with id {id} exist.");

            await this.contractDAO.RemoveContractAsync(id);
        }
    }
}
