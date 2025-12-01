using AutoMapper;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DAO.Interfaces;
using DormitoryManagementSystem.DTO.Admins;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.BUS.Implementations
{
    public class AdminBUS : IAdminBUS
    {
        private readonly IAdminDAO _adminDAO;
        private readonly IUserDAO _userDAO;
        private readonly IMapper _mapper;

        public AdminBUS(IAdminDAO adminDAO, IUserDAO userDAO, IMapper mapper)
        {
            this._adminDAO = adminDAO;
            this._userDAO = userDAO;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<AdminReadDTO>> GetAllAdminsAsync()
        {
            IEnumerable<Admin> admins = await this._adminDAO.GetAllAdminsAsync();
            return this._mapper.Map<IEnumerable<AdminReadDTO>>(admins);
        }

        public async Task<IEnumerable<AdminReadDTO>> GetAllAdminsIncludingInactivesAsync()
        {
            IEnumerable<Admin> admins = await this._adminDAO.GetAllAdminsIncludingInactivesAsync();
            return this._mapper.Map<IEnumerable<AdminReadDTO>>(admins);
        }

        public async Task<AdminReadDTO?> GetAdminByIDAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Admin id không thể để trống.");

            Admin? ad = await this._adminDAO.GetAdminByIDAsync(id);
            if (ad == null) return null;

            return this._mapper.Map<AdminReadDTO>(ad);
        }

        public async Task<AdminReadDTO?> GetAdminByUserIDAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("user id không thể để trống.");

            Admin? ad = await this._adminDAO.GetAdminByUserIDAsync(userId);
            if (ad == null) return null;

            return this._mapper.Map<AdminReadDTO>(ad);
        }

        public async Task<string> AddAdminAsync(AdminCreateDTO dto)
        {
            Admin? existing = await this._adminDAO.GetAdminByIDAsync(dto.AdminID);
            if (existing != null)
                throw new InvalidOperationException($"Không có quản trị viên với ID {dto.AdminID}.");

            Admin? existingCCCD = await this._adminDAO.GetAdminByCCCDAsync(dto.IDcard);
            if (existingCCCD != null)
                throw new InvalidOperationException($"Không có quản trị viên với CCCD {dto.IDcard}.");

            User? user = await this._userDAO.GetUserByIDAsync(dto.UserID);
            if (user == null)
                throw new KeyNotFoundException($"Không có người dùng với ID {dto.UserID}. Vui lòng tạo tài khoản người dùng trước.");

            if (user.Role != "Admin")
                throw new InvalidOperationException($"Người dùng {dto.UserID} có vai trò '{user.Role}'. Không thể gán hồ sơ quản trị viên.");

            Admin? existingProfile = await this._adminDAO.GetAdminByUserIDAsync(dto.UserID);
            if (existingProfile != null)
                throw new InvalidOperationException($"Người dùng {dto.UserID} đã được liên kết với hồ sơ quản trị viên {existingProfile.Adminid}.");

            Admin ad = this._mapper.Map<Admin>(dto);
            await this._adminDAO.AddAdminAsync(ad);

            return ad.Adminid;
        }

        public async Task UpdateAdminAsync(string id, AdminUpdateDTO dto)
        {
            Admin? currentAdmin = await _adminDAO.GetAdminByIDAsync(id);
            if (currentAdmin == null)
                throw new KeyNotFoundException($"Không có quản trị viên với ID {id}.");

            if (currentAdmin.Idcard != dto.IDcard)
            {
                Admin? existingCCCD = await this._adminDAO.GetAdminByCCCDAsync(dto.IDcard);
                if (existingCCCD != null)
                    throw new InvalidOperationException($"Không có quản trị viên với CCCD {dto.IDcard}.");
            }

            _mapper.Map(dto, currentAdmin);
            currentAdmin.Adminid = id;

            await _adminDAO.UpdateAdminAsync(currentAdmin);
        }

        public async Task DeleteAdminAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Admin ID không thể để trống.");

            Admin? existing = await _adminDAO.GetAdminByIDAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Không có quản trị viên với ID {id}.");

            // SOFT DELETE USER
            // Gọi UserDAO để set IsActive = false cho UserID tương ứng
            await _userDAO.DeleteUserAsync(existing.Userid);
        }
    }
}
