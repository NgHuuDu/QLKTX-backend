using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class ContractDAO : IContractDAO
    {
        private readonly DormitoryContext _context;

        public ContractDAO(DormitoryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Contract>> GetAllContractsAsync()
        {
            return await this._context.Contracts.ToListAsync();
        }

        public async Task<Contract?> GetContractByIDAsync(string id)
        {
            return await this._context.Contracts.FindAsync(id);
        }

        public async Task AddContractAsync(Contract contract)
        {
            await this._context.Contracts.AddAsync(contract);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            this._context.Contracts.Update(contract);
            await this._context.SaveChangesAsync();
        }

        public async Task RemoveContractAsync(string id)
        {
            Contract? c = await this._context.Contracts.FindAsync(id);

            if (c == null) return;
            this._context.Contracts.Remove(c);

            await this._context.SaveChangesAsync();
        }
    }
}
