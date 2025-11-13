using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;

namespace Dormitory.BUS.Implementations
{
    public class ViolationBUS
    {
        private readonly IViolationDAO violationDAO;

        public ViolationBUS(IViolationDAO violationDAO)
        {
            this.violationDAO = violationDAO;
        }

        public async Task<IEnumerable<Violation>> GetAllViolationsAsync()
        {
            return await this.violationDAO.GetAllViolationsAsync();
        }

        public async Task<Violation?> GetViolationByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Violation ID can not be null or empty.");

            return await this.violationDAO.GetViolationByIdAsync(id);
        }

        public async Task AddNewViolationAsync(Violation violation)
        {
            if (violation == null)
                throw new ArgumentNullException(nameof(violation));

            await this.violationDAO.AddNewViolationAsync(violation);
        }

        public async Task UpdateViolation(Violation violation)
        {
            if (violation == null)
                throw new ArgumentNullException(nameof(violation));

            Violation? v = await this.violationDAO.GetViolationByIdAsync(violation.Violationid);
            if (v == null)
                throw new InvalidOperationException($"No violation with id {violation.Violationid} exist.");

            await this.violationDAO.UpdateViolationAsync(violation);
        }

        public async Task RemoveViolationAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Violation ID can not be null or empty");

            Violation? v = await this.violationDAO.GetViolationByIdAsync(id);
            if (v == null)
                throw new InvalidOperationException($"No violation with id {id} exist.");

            await this.violationDAO.RemoveViolationAsync(id);
        }
    }
}
