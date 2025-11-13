using Dormitory.Models.Entities;

namespace Dormitory.BUS.Interfaces
{
    public interface IPaymentBUS
    {
        public Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        public Task<Payment?> GetPaymentByIDAsync(string id);
        public Task AddPaymentAsync(Payment payment);
        public Task UpdatePaymentAsync(Payment payment);
        public Task RemovePaymentAsync(string id);
    }
}
