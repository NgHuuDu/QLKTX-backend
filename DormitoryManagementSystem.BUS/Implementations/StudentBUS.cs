using AutoMapper;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DAO.Implementations;
using DormitoryManagementSystem.DAO.Interfaces;
using DormitoryManagementSystem.DTO.Students;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.BUS.Implementations
{
    public class StudentBUS : IStudentBUS
    {
        private readonly IStudentDAO _studentDAO;
        private readonly IContractDAO _contractDAO;
        private readonly IUserDAO _userDAO;
        private readonly IMapper _mapper;
        private readonly IPaymentDAO _paymentDAO;

        public StudentBUS(IStudentDAO studentDAO, IContractDAO contractDAO, IMapper mapper, IPaymentDAO paymentDAO)
        {
            this._studentDAO = studentDAO;
            this._contractDAO = contractDAO;
            this._paymentDAO = paymentDAO;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<StudentReadDTO>> GetAllStudentsAsync()
        {
            IEnumerable<Student> students = await this._studentDAO.GetAllStudentsAsync();
            return this._mapper.Map<IEnumerable<StudentReadDTO>>(students);
        }

        public async Task<IEnumerable<StudentReadDTO>> GetAllStudentsIncludingInactivesAsync()
        {
            IEnumerable<Student> students = await this._studentDAO.GetAllStudentsIncludingInactivesAsync();
            return this._mapper.Map<IEnumerable<StudentReadDTO>>(students);
        }

        public async Task<StudentReadDTO?> GetStudentByIDAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Student id không thể để trống");

            Student? student = await this._studentDAO.GetStudentByIDAsync(id);
            if (student == null) return null;

            return this._mapper.Map<StudentReadDTO>(student);
        }

        public async Task<StudentReadDTO?> GetStudentByCCCDAsync(string cccd)
        {
            if (string.IsNullOrWhiteSpace(cccd))
                throw new ArgumentException("Student cccd không thể để trống");

            Student? student = await this._studentDAO.GetStudentByCCCDAsync(cccd);
            if (student == null) return null;

            return this._mapper.Map<StudentReadDTO>(student);
        }

        public async Task<StudentReadDTO?> GetStudentByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Student email không thể để trống");

            Student? student = await this._studentDAO.GetStudentByEmailAsync(email);
            if (student == null) return null;

            return this._mapper.Map<StudentReadDTO>(student);
        }

        public async Task<string> AddStudentAsync(StudentCreateDTO dto)
        {
            var existingID = await _studentDAO.GetStudentByIDAsync(dto.StudentID);
            if (existingID != null)
                throw new InvalidOperationException($"Student với ID {dto.StudentID} đã tồn tại.");

            var existingCCCD = await _studentDAO.GetStudentByCCCDAsync(dto.CCCD);
            if (existingCCCD != null)
                throw new InvalidOperationException($"CCCD {dto.CCCD} đã được sử dụng bởi một sinh viên khác    .");

            if (!string.IsNullOrEmpty(dto.Email))
            {
                var existingEmail = await _studentDAO.GetStudentByEmailAsync(dto.Email);
                if (existingEmail != null)
                    throw new InvalidOperationException($"Email {dto.Email} đã được sử dụng bởi một sinh viên khác.");
            }

            Student student = _mapper.Map<Student>(dto);

            // LƯU Ý QUAN TRỌNG: 
            // Bảng Student yêu cầu UserID. Bạn cần logic để gán UserID vào đây.
            // Ví dụ: Nếu tạo Student cùng lúc tạo User, hoặc truyền UserID vào DTO.
            // Tạm thời code này giả định DTO hoặc logic khác đã xử lý việc này.

            await _studentDAO.AddStudentAsync(student);
            return student.Studentid;
        }

        public async Task UpdateStudentAsync(string id, StudentUpdateDTO dto)
        {
            Student? student = await _studentDAO.GetStudentByIDAsync(id);
            if (student == null)
                throw new KeyNotFoundException($"Không có student với ID {id}.");

            if (dto.CCCD != student.Idcard)
            {
                var duplicateCCCD = await _studentDAO.GetStudentByCCCDAsync(dto.CCCD);
                if (duplicateCCCD != null)
                    throw new InvalidOperationException($"CCCD {dto.CCCD} đã được sử dụng.");
            }

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != student.Email)
            {
                var duplicateEmail = await _studentDAO.GetStudentByEmailAsync(dto.Email);
                if (duplicateEmail != null)
                    throw new InvalidOperationException($"Email {dto.Email} đã được sử dụng.");
            }

            _mapper.Map(dto, student);
            student.Studentid = id;

            await _studentDAO.UpdateStudentAsync(student);
        }

        public async Task DeleteStudentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("ID không thể để trống");

            var student = await _studentDAO.GetStudentByIDAsync(id);
            if (student == null) throw new KeyNotFoundException($"Không có student {id}.");

            var activeContract = await _contractDAO.GetActiveContractByStudentIDAsync(id);
            if (activeContract != null)
            {
                throw new InvalidOperationException($"Không thể xóa student {id} vì họ có hợp đồng đang hoạt động trong phòng {activeContract.Roomid}. " +
                                                    $"Vui lòng kết thúc hợp đồng trước.");
            }

            // THAY VÌ XÓA STUDENT -> TA SOFT DELETE USER CỦA HỌ
            // Khi User bị set IsActive=false, thì hàm GetAllActiveStudentsAsync sẽ tự động lọc sinh viên này ra.
            await _userDAO.DeleteUserAsync(student.Userid);
        }




        //Student
        //Mới thêm hàm lấy profile sinh viên
        public async Task<StudentProfileDTO?> GetStudentProfileAsync(string studentId)
        {
            var student = await _studentDAO.GetStudentByIDAsync(studentId);
            if (student == null) return null;

            var activeContract = await _contractDAO.GetContractDetailAsync(studentId);
            
            

            var profile = new StudentProfileDTO
            {
                StudentID = student.Studentid,
                FullName = student.Fullname,
                Major = student.Major ?? "N/A",

                DateOfBirth = student.Dateofbirth.HasValue ? student.Dateofbirth.Value.ToString("dd/MM/yyyy") : "N/A",
                PhoneNumber = student.Phonenumber,
                Gender = student.Gender,
                Email = student.Email,
                CCCD = student.Idcard, 
                Address = student.Address
            };

            if (activeContract != null)
            {
                if (activeContract.Room != null)
                {
                    profile.RoomName = activeContract.Room.Roomnumber.ToString();
                    profile.BuildingName = activeContract.Room.Building != null
                                           ? activeContract.Room.Building.Buildingname
                                           : "Unknown";
                }
                profile.ContractStatus = activeContract.Status;

                var payments = await _paymentDAO.GetPaymentsByContractIDAsync(activeContract.Contractid);

                decimal totalDebt = payments
                                    .Where(p => p.Paymentstatus == "Unpaid" || p.Paymentstatus == "Late")
                                    .Sum(p => p.Paymentamount);

                profile.TotalDebt = totalDebt;

                profile.AmountToPay = totalDebt;

                if (totalDebt > 0)
                {
                    profile.IsDebt = true; 
                    profile.PaymentStatusDisplay = "Chưa thanh toán";
                }
                else
                {
                    bool hasBills = payments.Any();
                    if (hasBills)
                    {
                        profile.IsDebt = false; // Không nợ -> Màu xanh
                        profile.PaymentStatusDisplay = "Đã thanh toán";
                    }
                    else
                    {
                        profile.IsDebt = false;
                        profile.PaymentStatusDisplay = "Chưa có hóa đơn";
                    }
                }
            }
            else
            {
                profile.PaymentStatusDisplay = "Chưa đăng ký phòng";
            }

            return profile;
        }

        public async Task UpdateContactInfoAsync(string studentId, StudentContactUpdateDTO dto)
        {
            var student = await _studentDAO.GetStudentByIDAsync(studentId);
            if (student == null)
                throw new KeyNotFoundException($"Không tìm thấy sinh viên với ID {studentId}.");
           


            if (!string.Equals(student.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var duplicateEmail = await _studentDAO.GetStudentByEmailAsync(dto.Email);
                if (duplicateEmail != null)
                {
                    throw new InvalidOperationException($"Email '{dto.Email}' đã được sử dụng bởi sinh viên khác.");
                }
            }

            student.Phonenumber = dto.PhoneNumber;
            student.Email = dto.Email;
            student.Address = dto.Address;

            await _studentDAO.UpdateStudentAsync(student);
        }

    }
}
