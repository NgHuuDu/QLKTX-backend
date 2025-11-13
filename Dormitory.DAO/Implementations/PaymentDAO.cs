using Dormitory.DAO.Interfaces;
using Dormitory.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dormitory.DAO.Implementations
{
    public class PaymentDAO : IPaymentDAO
    {
        private readonly DormitoryContext _context;

        public PaymentDAO(DormitoryContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await this._context.Payments.ToListAsync();
        }

        public async Task<Payment?> GetPaymentByIDAsync(string id)
        {
            return await this._context.Payments.FindAsync(id);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await this._context.Payments.AddAsync(payment);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            this._context.Payments.Update(payment);
            await this._context.SaveChangesAsync();
        }

        public async Task RemovePaymentAsync(string id)
        {
            Payment? p = await this._context.Payments.FindAsync(id);

            if (p == null) return;
            this._context.Payments.Remove(p);

            await this._context.SaveChangesAsync(); 
        }
    }
}
