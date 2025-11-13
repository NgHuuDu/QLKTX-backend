using Dormitory.Models.Entities;

namespace Dormitory.DAO.Interfaces
{
    public interface IPaymentDAO
    {
        public Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        public Task<Payment?> GetPaymentByIDAsync(string id);
        public Task AddPaymentAsync(Payment payment);
        public Task UpdatePaymentAsync(Payment payment);
        public Task RemovePaymentAsync(string id);
    }
}
