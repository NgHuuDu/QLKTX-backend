using DormitoryManagementSystem.DTO.Payments;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.BUS.Interfaces
{
    public interface IPaymentBUS
    {
        Task<IEnumerable<PaymentReadDTO>> GetAllPaymentsAsync();
        Task<PaymentReadDTO?> GetPaymentByIDAsync(string id);
        Task<IEnumerable<PaymentReadDTO>> GetPaymentsByContractIDAsync(string contractId);
        Task<IEnumerable<PaymentReadDTO>> GetPaymentsByStatusAsync(string status);
        Task<string> AddPaymentAsync(PaymentCreateDTO dto);
        Task UpdatePaymentAsync(string id, PaymentUpdateDTO dto);
        Task DeletePaymentAsync(string id);



        //Student
        // Lấy danh sách thanh toán của sinh viên (Lọc theo trạng thái: All, Paid, Pending)
        Task<IEnumerable<PaymentListDTO>> GetPaymentsByStudentAndStatusAsync(string studentId, string? status);





        // Admin
        // Lấy danh sách thanh toán với các bộ lọc (tháng, trạng thái, tìm kiếm)
        Task<IEnumerable<PaymentAdminDTO>> GetPaymentsForAdminAsync(int? month, string? status, string? building, string? search);

        // Xác nhận thanh toán
        Task ConfirmPaymentAsync(string id, PaymentConfirmDTO dto);


        Task<int> GenerateMonthlyBillsAsync(int month, int year);

    }
}