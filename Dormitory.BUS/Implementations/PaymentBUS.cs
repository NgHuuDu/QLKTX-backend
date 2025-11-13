using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;

namespace Dormitory.BUS.Implementations
{
    public class PaymentBUS
    {
        private readonly IPaymentDAO paymentDAO;

        public PaymentBUS(IPaymentDAO paymentDAO)
        {
            this.paymentDAO = paymentDAO;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await this.paymentDAO.GetAllPaymentsAsync();
        }

        public async Task<Payment?> GetPaymentByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Payment ID can not be null or empty.");

            return await this.paymentDAO.GetPaymentByIDAsync(id);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            await this.paymentDAO.AddPaymentAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            Payment? p = await this.paymentDAO.GetPaymentByIDAsync(payment.Paymentid);
            if (p == null)
                throw new InvalidOperationException($"No payment with id {payment.Paymentid} exist.");

            await this.paymentDAO.UpdatePaymentAsync(payment);
        }

        public async Task RemovePaymentAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("Payment ID can not be null or empty.");

            Payment? p = await this.paymentDAO.GetPaymentByIDAsync(id);
            if (p == null)
                throw new InvalidOperationException($"No payment with id {id} exist.");

            await this.paymentDAO.RemovePaymentAsync(id);
        }
    }
}
