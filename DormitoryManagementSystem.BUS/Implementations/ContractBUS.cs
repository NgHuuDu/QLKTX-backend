using AutoMapper;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DAO.Interfaces;
using DormitoryManagementSystem.DTO.Contracts;
using DormitoryManagementSystem.Entity;

namespace DormitoryManagementSystem.BUS.Implementations
{
    public class ContractBUS : IContractBUS
    {
        private readonly IContractDAO _contractDAO;
        private readonly IStudentDAO _studentDAO;
        private readonly IRoomDAO _roomDAO;
        private readonly IUserDAO _userDAO;
        private readonly IMapper _mapper;

        public ContractBUS(
            IContractDAO contractDAO,
            IStudentDAO studentDAO,
            IRoomDAO roomDAO,
            IUserDAO userDAO,
            IMapper mapper)
        {
            _contractDAO = contractDAO;
            _studentDAO = studentDAO;
            _roomDAO = roomDAO;
            _userDAO = userDAO;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContractReadDTO>> GetAllContractsAsync()
        {
            IEnumerable<Contract> contracts = await _contractDAO.GetAllContractsAsync();
            return _mapper.Map<IEnumerable<ContractReadDTO>>(contracts);
        }

        public async Task<IEnumerable<ContractReadDTO>> GetAllContractsIncludingInactivesAsync()
        {
            IEnumerable<Contract> contracts = await _contractDAO.GetAllContractsIncludingInactivesAsync();
            return _mapper.Map<IEnumerable<ContractReadDTO>>(contracts);
        }

        public async Task<ContractReadDTO?> GetContractByIDAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Contract ID không thể để trống");

            Contract? contract = await _contractDAO.GetContractByIDAsync(id);
            if (contract == null) return null;

            return _mapper.Map<ContractReadDTO>(contract);
        }

        public async Task<IEnumerable<ContractReadDTO>> GetContractsByStudentIDAsync(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Student ID không thể để trống");

            IEnumerable<Contract> contracts = await _contractDAO.GetContractsByStudentIDAsync(studentId);
            return _mapper.Map<IEnumerable<ContractReadDTO>>(contracts);
        }

        public async Task<ContractReadDTO?> GetActiveContractByStudentIDAsync(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new ArgumentException("Student ID không thể để trống");

            Contract? contract = await _contractDAO.GetActiveContractByStudentIDAsync(studentId);
            if (contract == null) return null;

            return _mapper.Map<ContractReadDTO>(contract);
        }

        /* AddContract củ
        public async Task<string> AddContractAsync(ContractCreateDTO dto)
        {
            Contract? existingContract = await _contractDAO.GetContractByIDAsync(dto.ContractID);
            if (existingContract != null)
                throw new InvalidOperationException($"Contract with ID {dto.ContractID} already exists.");

            if (dto.EndTime <= dto.StartTime)
                throw new ArgumentException("End Date must be after Start Date.");

            Student? student = await _studentDAO.GetStudentByIDAsync(dto.StudentID);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {dto.StudentID} not found.");

            Contract? activeContract = await _contractDAO.GetActiveContractByStudentIDAsync(dto.StudentID);
            if (activeContract != null)
                throw new InvalidOperationException($"Student {dto.StudentID} already has an active contract in room {activeContract.Roomid}.");

            if (!string.IsNullOrEmpty(dto.StaffUserID))
            {
                User? staff = await _userDAO.GetUserByIDAsync(dto.StaffUserID);
                if (staff == null || staff.IsActive == false)
                    throw new InvalidOperationException("Staff User is invalid or inactive.");
            }

            Room? room = await _roomDAO.GetRoomByIDAsync(dto.RoomID);
            if (room == null)
                throw new KeyNotFoundException($"Room with ID {dto.RoomID} not found.");

            if (room.Status != "Active")
                throw new InvalidOperationException($"Room {dto.RoomID} is not Active (Status: {room.Status}).");

            // Hợp đồng Pending không tăng occupancy nên có thể tạo ngay cả khi phòng đầy
            if (dto.Status == "Active" && room.Currentoccupancy >= room.Capacity)
                throw new InvalidOperationException($"Room {dto.RoomID} is full. Capacity: {room.Capacity}.");

            Contract contractEntity = _mapper.Map<Contract>(dto);
            contractEntity.Createddate = DateTime.Now;

            await _contractDAO.AddContractAsync(contractEntity);

            // Tăng occupancy khi hợp đồng là Active
            if (contractEntity.Status == "Active")
            {
                room.Currentoccupancy += 1;
                await _roomDAO.UpdateRoomAsync(room);
            }

            return contractEntity.Contractid;
        }
        */

        public async Task<string> AddContractAsync(ContractCreateDTO dto, string staffuserID)
        {
            Contract? existingContract = await _contractDAO.GetContractByIDAsync(dto.ContractID);
            if (existingContract != null)
                throw new InvalidOperationException($"Hợp đồng với ID {dto.ContractID} đã tồn tại.");

            if (dto.EndTime <= dto.StartTime)
                throw new ArgumentException("Ngày kết thúc phải sau ngày bắt đầu.");

            Student? student = await _studentDAO.GetStudentByIDAsync(dto.StudentID);
            if (student == null)
                throw new KeyNotFoundException($"Không có sinh viên với ID {dto.StudentID}.");

            Contract? activeContract = await _contractDAO.GetActiveContractByStudentIDAsync(dto.StudentID);
            if (activeContract != null)
                throw new InvalidOperationException($"Sinh viên {dto.StudentID} đã có hợp đồng hoạt động trong phòng {activeContract.Roomid}.");

            if (!string.IsNullOrEmpty(dto.StaffUserID))
            {
                User? staff = await _userDAO.GetUserByIDAsync(dto.StaffUserID);
                if (staff == null || staff.IsActive == false)
                    throw new InvalidOperationException("Tài khoản nhân viên không hợp lệ hoặc không hoạt động.");
            }

            Room? room = await _roomDAO.GetRoomByIDAsync(dto.RoomID);
            if (room == null)
                throw new KeyNotFoundException($"Không có phòng với ID {dto.RoomID}.");

            if (room.Status != "Active")
                throw new InvalidOperationException($"Phòng {dto.RoomID} không hoạt động (Trạng thái: {room.Status}).");

            if (room.Currentoccupancy >= room.Capacity)
                throw new InvalidOperationException($"Phòng {dto.RoomID} đã đầy. Sức chứa: {room.Capacity}.");

            Contract contractEntity = _mapper.Map<Contract>(dto);
            contractEntity.Createddate = DateTime.Now;
            contractEntity.Staffuserid = staffuserID;

            await _contractDAO.AddContractAsync(contractEntity);

            // Increase occupancy when contract is Active
            if (contractEntity.Status == "Active")
            {
                room.Currentoccupancy += 1;
                await _roomDAO.UpdateRoomAsync(room);
            }

            return contractEntity.Contractid;
        }



        public async Task UpdateContractAsync(string id, ContractUpdateDTO dto)
        {
            Contract? contractEntity = await _contractDAO.GetContractByIDAsync(id);
            if (contractEntity == null)
                throw new KeyNotFoundException($"Không có hợp đồng với ID {id}.");

            if (dto.EndTime <= dto.StartTime)
                throw new ArgumentException("Ngày kết thúc phải sau ngày bắt đầu.");

            // Handle Logic if Room is Changed or Status is Changed
            string oldRoomID = contractEntity.Roomid;
            string newRoomID = dto.RoomID;
            string oldStatus = contractEntity.Status ?? "Active";
            string newStatus = dto.Status;

            bool isRoomChanged = oldRoomID != newRoomID;
            bool isStatusChanged = oldStatus != newStatus;

            // Case A: Room Transfer (Moving from Old Room -> New Room)
            if (isRoomChanged)
            {
                // Decrease occupancy in Old Room (if it was Active)
                if (oldStatus == "Active")
                {
                    Room? oldRoom = await _roomDAO.GetRoomByIDAsync(oldRoomID);
                    if (oldRoom != null && oldRoom.Currentoccupancy > 0)
                    {
                        oldRoom.Currentoccupancy -= 1;
                        await _roomDAO.UpdateRoomAsync(oldRoom);
                    }
                }

                // Increase occupancy in New Room (if new status is Active)
                if (newStatus == "Active")
                {
                    Room? newRoom = await _roomDAO.GetRoomByIDAsync(newRoomID);
                    if (newRoom == null) throw new KeyNotFoundException($"Không có phòng {newRoomID}.");

                    if (newRoom.Currentoccupancy >= newRoom.Capacity)
                        throw new InvalidOperationException($"Phòng {newRoomID} đã đầy.");

                    newRoom.Currentoccupancy += 1;
                    await _roomDAO.UpdateRoomAsync(newRoom);
                }
            }
            // Case B: Same Room, but Status Changed
            else if (isStatusChanged)
            {
                Room? currentRoom = await _roomDAO.GetRoomByIDAsync(oldRoomID);
                if (currentRoom != null)
                {
                    if (oldStatus == "Active" && newStatus != "Active")
                    {
                        if (currentRoom.Currentoccupancy > 0)
                            currentRoom.Currentoccupancy -= 1;
                    }
                    else if (oldStatus != "Active" && newStatus == "Active")
                    {
                        if (currentRoom.Currentoccupancy >= currentRoom.Capacity)
                            throw new InvalidOperationException($"Phòng {currentRoom.Roomid} đã đầy.");
                        currentRoom.Currentoccupancy += 1;
                    }
                    await _roomDAO.UpdateRoomAsync(currentRoom);
                }
            }

            _mapper.Map(dto, contractEntity);
            contractEntity.Contractid = id; 

            await _contractDAO.UpdateContractAsync(contractEntity);
        }

        public async Task DeleteContractAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Contract ID không thể để trống");

            Contract? contract = await _contractDAO.GetContractByIDAsync(id);
            if (contract == null)
                throw new KeyNotFoundException($"Không có hợp đồng với ID {id}.");

            if (contract.Status == "Active")
            {
                Room? room = await _roomDAO.GetRoomByIDAsync(contract.Roomid);
                if (room != null && room.Currentoccupancy > 0)
                {
                    room.Currentoccupancy -= 1;
                    await _roomDAO.UpdateRoomAsync(room);
                }
            }

            // Soft delete via DAO (sets status to Terminated)
            await _contractDAO.DeleteContractAsync(id);
        }

        //Student
        // Lấy chi tiết hợp đồng đầy đủ bao gồm thông tin sinh viên và phòng
        // Của thằng học sinh đó
        public async Task<ContractDetailDTO?> GetContractFullDetailAsync(string studentId)
        {
            var contract = await _contractDAO.GetContractDetailAsync(studentId);

            if (contract == null) return null; 

            return new ContractDetailDTO
            {
                // Hợp đồng
                ContractID = contract.Contractid,
                StartTime = contract.Starttime.ToDateTime(TimeOnly.MinValue),
                EndTime = contract.Endtime.ToDateTime(TimeOnly.MinValue),
                Status = contract.Status,
                //CreatedDate = contract.Createddate ?? DateTime.MinValue,

                // Sinh viên 
                StudentID = contract.Studentid,
                StudentName = contract.Student?.Fullname ?? "N/A",
                Gender = contract.Student?.Gender ?? "N/A",
                Major = contract.Student?.Major ?? "N/A",
                PhoneNumber = contract.Student?.Phonenumber ?? "N/A",
                Email = contract.Student?.Email ?? "N/A",
                CCCD = contract.Student?.Idcard ?? "N/A",
                Address = contract.Student?.Address ?? "N/A",

                // Phòng
                RoomID = contract.Roomid,
                RoomNumber = contract.Room?.Roomnumber ?? 0,
                //Price = contract.Room?.Price ?? 0,
                BuildingName = contract.Room?.Building?.Buildingname ?? "Unknown Building"
            };
        }


        public async Task<string> RegisterContractAsync(string studentId, ContractRegisterDTO dto)
        {
            if (dto.EndTime <= dto.StartTime)
                throw new ArgumentException("Ngày kết thúc phải sau ngày bắt đầu.");

            var activeContract = await _contractDAO.GetActiveContractByStudentIDAsync(studentId);
            if (activeContract != null)
                throw new InvalidOperationException("Bạn hiện đang có hợp đồng hiệu lực, không thể đăng ký mới.");

            var room = await _roomDAO.GetRoomByIDAsync(dto.RoomID);
            if (room == null) throw new KeyNotFoundException("Phòng không tồn tại.");

            if (room.Status != "Active")
                throw new InvalidOperationException("Phòng này đang bảo trì.");

            if (room.Currentoccupancy >= room.Capacity)
                throw new InvalidOperationException("Phòng này đã đầy.");

            // Tạo Entity Hợp đồng Mới
            var newContract = new Contract
            {
                // Tự sinh mã HĐ: VD: CTR_20241125_123456
                Contractid = $"CTR_{DateTime.Now:yyyyMMdd}_{new Random().Next(1000, 9999)}",
                Studentid = studentId,
                Roomid = dto.RoomID,
                Starttime = DateOnly.FromDateTime(dto.StartTime), 
                Endtime = DateOnly.FromDateTime(dto.EndTime),
                Status = "Pending", 
                Createddate = DateTime.Now
            };

            await _contractDAO.AddContractAsync(newContract);

            // Lúc này là chưa tăng cái CurrentOccupancy của phòng vì HĐ đang ở trạng thái Pending
            // Chờ admin duyệt rồi mới tăng

            return newContract.Contractid;
        }



        // Admin thêm chức năng lọc theo tên và mã sv
        // ucContractManagement
        public async Task<IEnumerable<ContractReadDTO>> GetContractsAsync(string SearchTerm)
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                SearchTerm = SearchTerm.Trim().ToLower();
            }
            else
            {
                // Nếu chỉ toàn khoảng trắng hoặc null -> Gán null luôn để DAO dễ check
                SearchTerm = null;
            }
            var contracts = await _contractDAO.GetContractsByFilterAsync(SearchTerm);
            return _mapper.Map<IEnumerable<ContractReadDTO>>(contracts);
        }

        // frmFilterContract

        public async Task<IEnumerable<ContractReadDTO>> GetContractsByMultiConditionAsync(ContractFilterDTO filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Status))
            {
                filter.Status = "All";
            }
            if (string.Equals(filter.BuildingID, "All", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(filter.BuildingID, "Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                filter.BuildingID = null;
            }

            if (string.Equals(filter.Status, "All", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(filter.Status, "Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                filter.Status = null;
            }

            if (filter.FromDate.HasValue && filter.ToDate.HasValue)
            {
                if (filter.FromDate.Value > filter.ToDate.Value)
                {
                    var temp = filter.FromDate;
                    filter.FromDate = filter.ToDate;
                    filter.ToDate = temp;
                }
            }

            var contracts = await _contractDAO.GetContractsByMultiConditionAsync(filter);

            return _mapper.Map<IEnumerable<ContractReadDTO>>(contracts);
        }
    }
}