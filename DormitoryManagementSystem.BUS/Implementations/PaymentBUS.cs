using AutoMapper;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DAO.Implementations;
using DormitoryManagementSystem.DAO.Interfaces;
using DormitoryManagementSystem.DTO.Payments;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.BUS.Implementations
{
    public class PaymentBUS : IPaymentBUS
    {
        private readonly IPaymentDAO _paymentDAO;
        private readonly IContractDAO _contractDAO;
        private readonly IMapper _mapper;
        private readonly IRoomDAO _roomDAO;

        public PaymentBUS(IPaymentDAO paymentDAO, IContractDAO contractDAO, IMapper mapper, IRoomDAO roomDAO)
        {
            _paymentDAO = paymentDAO;
            _contractDAO = contractDAO;
            _mapper = mapper;
            _roomDAO = roomDAO;
        }

        public async Task<IEnumerable<PaymentReadDTO>> GetAllPaymentsAsync()
        {
            IEnumerable<Payment> payments = await _paymentDAO.GetAllPaymentsAsync();
            return _mapper.Map<IEnumerable<PaymentReadDTO>>(payments);
        }

        public async Task<PaymentReadDTO?> GetPaymentByIDAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Payment ID không thể để trống");

            Payment? payment = await _paymentDAO.GetPaymentByIDAsync(id);
            if (payment == null) return null;

            return _mapper.Map<PaymentReadDTO>(payment);
        }

        public async Task<IEnumerable<PaymentReadDTO>> GetPaymentsByContractIDAsync(string contractId)
        {
            if (string.IsNullOrWhiteSpace(contractId))
                throw new ArgumentException("Contract ID không thể để trống");

            Contract? contract = await _contractDAO.GetContractByIDAsync(contractId);
            if (contract == null)
                throw new KeyNotFoundException($"Không có hợp đồng với ID {contractId}.");

            IEnumerable<Payment> payments = await _paymentDAO.GetPaymentsByContractIDAsync(contractId);
            return _mapper.Map<IEnumerable<PaymentReadDTO>>(payments);
        }

        public async Task<IEnumerable<PaymentReadDTO>> GetPaymentsByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status không thể để trống");

            IEnumerable<Payment> payments = await _paymentDAO.GetPaymentsByStatusAsync(status);
            return _mapper.Map<IEnumerable<PaymentReadDTO>>(payments);
        }

        public async Task<string> AddPaymentAsync(PaymentCreateDTO dto)
        {
            Payment? existingPayment = await _paymentDAO.GetPaymentByIDAsync(dto.PaymentID);
            if (existingPayment != null)
                throw new InvalidOperationException($"Payment với ID {dto.PaymentID} đã tồn tại.");

            Contract? contract = await _contractDAO.GetContractByIDAsync(dto.ContractID);
            if (contract == null)
                throw new KeyNotFoundException($"Không có hợp đồng với ID {dto.ContractID}.");

            if (dto.PaidAmount > dto.PaymentAmount)
                throw new InvalidOperationException("Số tiền đã thanh toán không thể vượt quá số tiền yêu cầu.");

            // Auto-update Status based on Paid Amount if logic dictates
            // If user sends "Unpaid" but PaidAmount == PaymentAmount, we might want to correct it.
            // However, we will respect the DTO's Status unless it's clearly invalid logic.
            if (dto.PaidAmount >= dto.PaymentAmount && dto.PaymentStatus == "Unpaid")
                dto.PaymentStatus = "Paid";

            Payment paymentEntity = _mapper.Map<Payment>(dto);

            if (paymentEntity.Paymentdate == null)
            {
                paymentEntity.Paymentdate = DateTime.Now;
            }

            await _paymentDAO.AddPaymentAsync(paymentEntity);

            return paymentEntity.Paymentid;
        }

        public async Task UpdatePaymentAsync(string id, PaymentUpdateDTO dto)
        {
            Payment? paymentEntity = await _paymentDAO.GetPaymentByIDAsync(id);
            if (paymentEntity == null)
                throw new KeyNotFoundException($"Không có payment với ID {id}.");

            if (dto.PaidAmount > paymentEntity.Paymentamount)
                throw new InvalidOperationException($"Số tiền đã thanh toán ({dto.PaidAmount}) không thể vượt quá số tiền gốc ({paymentEntity.Paymentamount}).");

            _mapper.Map(dto, paymentEntity);

            paymentEntity.Paymentid = id;

            // Auto-correct Status logic (Optional convenience)
            if (paymentEntity.Paidamount >= paymentEntity.Paymentamount && paymentEntity.Paymentstatus != "Paid")
            {
                paymentEntity.Paymentstatus = "Paid";
            }
            else if (paymentEntity.Paidamount < paymentEntity.Paymentamount && paymentEntity.Paymentstatus == "Paid")
            {
                // If someone updates it to 'Paid' but amount is insufficient, warn or revert?
                // Here we trust the DTO status but it's good to be aware.
                // Let's allow it for now in case of manual override.
            }

            await _paymentDAO.UpdatePaymentAsync(paymentEntity);
        }

        public async Task DeletePaymentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Payment ID không thể để trống");

            Payment? payment = await _paymentDAO.GetPaymentByIDAsync(id);
            if (payment == null)
                throw new KeyNotFoundException($"Không có payment với ID {id}.");

            await _paymentDAO.RemovePaymentAsync(id);
        }




        //Student
        // Lấy các hóa đơn  của SV dựa trên trạng thái 
        public async Task<IEnumerable<PaymentListDTO>> GetPaymentsByStudentAndStatusAsync(string studentId, string? status)
        {

            var payments = await _paymentDAO.GetPaymentsByStudentAndStatusAsync(studentId, status);


            var result = payments.Select(p => new PaymentListDTO
            {
                PaymentID = p.Paymentid,

                BillMonth = p.Billmonth,

                PaymentAmount = p.Paymentamount,

                PaymentStatus = p.Paymentstatus ?? "Unknown",

                Description = p.Description ?? "",

                PaymentDate = p.Paymentdate
            });

            return result;
        }
        // Admin
        // Lấy các hóa đơn với bộ lọc cho Admin
        public async Task<IEnumerable<PaymentAdminDTO>> GetPaymentsForAdminAsync(
        int? month, string? status, string? building, string? search)
        {
            var payments = await _paymentDAO.GetPaymentsForAdminAsync(month, status, building, search);

            return payments.Select(p => new PaymentAdminDTO
            {
                PaymentID = p.Paymentid,
                ContractID = p.Contractid,

                StudentID = p.Contract.Studentid,
                StudentName = p.Contract.Student?.Fullname ?? "Unknown",
                RoomName = p.Contract.Room?.Roomnumber.ToString() ?? "N/A",

                BillMonth = p.Billmonth,
                PaymentAmount = p.Paymentamount,
                PaymentStatus = p.Paymentstatus ?? "Unpaid",
                PaymentDate = p.Paymentdate,
                PaymentMethod = p.Paymentmethod
            });
        }


        // Xác nhận thanh toán
        public async Task ConfirmPaymentAsync(string id, PaymentConfirmDTO dto)
        {
            //  Tìm hóa đơn
            var payment = await _paymentDAO.GetPaymentByIDAsync(id);
            if (payment == null)
                throw new KeyNotFoundException($"Không tìm thấy hóa đơn {id}");

            if (payment.Paymentstatus == "Paid")
                throw new InvalidOperationException("Hóa đơn này đã thanh toán rồi.");

            // Cập nhật thông tin QUAN TRỌNG
            payment.Paymentstatus = "Paid";
            payment.Paymentdate = DateTime.Now; // Ngày giờ thực tế admin bấm nút

            // Cập nhật Phương thức (Cash/Bank Transfer) lấy từ DTO
            payment.Paymentmethod = dto.PaymentMethod;

            // Xử lý Ghi chú (Lưu mã giao dịch)
            // Nếu có Note thì nối thêm vào Description cũ (hoặc ghi đè tùy bạn)
            if (!string.IsNullOrEmpty(dto.Note))
            {
                // Ví dụ: "Tiền điện T9 (Mã CK: 123456789)"
                payment.Description = string.IsNullOrEmpty(payment.Description)
                    ? dto.Note
                    : $"{payment.Description} | Note: {dto.Note}";
            }

            // Tự động set Full tiền (vì xác nhận là đã trả hết)
            if (payment.Paidamount < payment.Paymentamount)
            {
                payment.Paidamount = payment.Paymentamount;
            }

            await _paymentDAO.UpdatePaymentAsync(payment);
        }

        public async Task<int> GenerateMonthlyBillsAsync(int month, int year)
        {
            var activeContracts = await _contractDAO.GetAllContractsAsync();
            activeContracts = activeContracts.Where(c => c.Status == "Active").ToList();

            int count = 0;

            foreach (var contract in activeContracts)
            {
                var exist = await _paymentDAO.GetPaymentsByContractIDAsync(contract.Contractid);
                bool isExist = exist.Any(p => p.Billmonth == month && p.Paymentdate.Value.Year == year);

                if (!isExist)
                {
                    var room = await _roomDAO.GetRoomByIDAsync(contract.Roomid);
                    decimal roomPrice = room?.Price ?? 0;

                    var newBill = new PaymentCreateDTO
                    {
                        PaymentID = $"PAY_{year}{month}_{contract.Contractid}",
                        ContractID = contract.Contractid,
                        BillMonth = month,
                        PaymentAmount = roomPrice,
                        PaymentStatus = "Unpaid",
                        Description = $"Tiền phòng tháng {month}/{year}",
                        PaymentDate = null
                    };

                    await AddPaymentAsync(newBill);
                    count++;
                }



            }

            return count;
        }
    }
}