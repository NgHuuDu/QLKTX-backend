using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DormitoryManagementSystem.API.Models;
using DormitoryManagementSystem.BUS.Interfaces;
using DormitoryManagementSystem.DTO.Buildings;
using DormitoryManagementSystem.DTO.Contracts;
using DormitoryManagementSystem.DTO.Rooms;
using DormitoryManagementSystem.DTO.Violations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryManagementSystem.API.Controllers
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class DashboardController : ControllerBase
    {
        private readonly IRoomBUS _roomBUS;
        private readonly IBuildingBUS _buildingBUS;
        private readonly IContractBUS _contractBUS;
        private readonly IPaymentBUS _paymentBUS;
        private readonly IViolationBUS _violationBUS;

        public DashboardController(
            IRoomBUS roomBUS,
            IBuildingBUS buildingBUS,
            IContractBUS contractBUS,
            IPaymentBUS paymentBUS,
            IViolationBUS violationBUS)
        {
            _roomBUS = roomBUS;
            _buildingBUS = buildingBUS;
            _contractBUS = contractBUS;
            _paymentBUS = paymentBUS;
            _violationBUS = violationBUS;
        }

        [HttpGet("buildings")]
        public async Task<ActionResult<BuildingKpiResponse>> GetBuildingKpis()
        {
            var rooms = await _roomBUS.GetAllRoomsIncludingInactivesAsync();
            var buildings = (await _buildingBUS.GetAllBuildingAsync())
                .ToDictionary(b => b.BuildingID, b => b, StringComparer.OrdinalIgnoreCase);

            var response = new BuildingKpiResponse
            {
                Buildings = rooms
                    .GroupBy(r => r.BuildingID)
                    .Select(group =>
                    {
                        buildings.TryGetValue(group.Key, out var buildingDto);
                        var totalBeds = group.Sum(r => r.Capacity);
                        var occupiedBeds = group.Sum(r => r.CurrentOccupancy);

                        return new BuildingKpiModel
                        {
                            BuildingName = buildingDto?.BuildingName ?? group.Key,
                            Gender = buildingDto?.Gender ?? "N/A",
                            Floors = EstimateFloors(group.Count()),
                            OccupiedRooms = occupiedBeds,
                            TotalRooms = totalBeds,
                            OccupancyRate = totalBeds == 0
                                ? 0
                                : Math.Round((decimal)occupiedBeds * 100 / totalBeds, 2)
                        };
                    })
                    .OrderByDescending(kpi => kpi.OccupancyRate)
                    .ToList()
            };

            return Ok(response);
        }

        [HttpGet("kpis")]
        public async Task<ActionResult<DashboardKpiResponse>> GetDashboardKpis(
            [FromQuery] string? building,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var (rooms, buildings) = await LoadRoomsAsync();
            var filteredRooms = FilterRoomsByBuilding(rooms, buildings, building).ToList();
            var roomIds = filteredRooms.Select(r => r.RoomID).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var contracts = (await _contractBUS.GetAllContractsAsync())
                .Where(c => roomIds.Contains(c.RoomID));
            var payments = await _paymentBUS.GetAllPaymentsAsync();
            var violations = (await _violationBUS.GetAllViolationsAsync())
                .Where(v => roomIds.Contains(v.RoomID));

            var dateFrom = from?.Date ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dateTo = to?.Date ?? DateTime.Now.Date;

            var roomsTotal = filteredRooms.Count;
            var roomsOccupied = filteredRooms.Count(r => r.CurrentOccupancy >= r.Capacity);
            var roomsAvailable = Math.Max(roomsTotal - roomsOccupied, 0);

            var paymentsThisPeriod = payments
                .Where(p => p.PaymentDate.Date >= dateFrom && p.PaymentDate.Date <= dateTo)
                .Sum(p => p.PaymentAmount);

            var response = new DashboardKpiResponse
            {
                RoomsTotal = roomsTotal,
                RoomsAvailable = roomsAvailable,
                RoomsOccupied = roomsOccupied,
                ContractsPending = contracts.Count(c => IsPendingStatus(c.Status)),
                PaymentsThisMonth = paymentsThisPeriod,
                ViolationsOpen = violations.Count(v => !IsClosedViolation(v.Status))
            };

            return Ok(response);
        }

        [HttpGet("charts")]
        public async Task<ActionResult<DashboardChartsResponse>> GetDashboardCharts(
            [FromQuery] string? building,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var (rooms, buildings) = await LoadRoomsAsync();
            var filteredRooms = FilterRoomsByBuilding(rooms, buildings, building).ToList();
            var roomIds = filteredRooms.Select(r => r.RoomID).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var contracts = (await _contractBUS.GetAllContractsAsync())
                .Where(c => roomIds.Contains(c.RoomID));

            var dateFrom = from?.Date ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dateTo = to?.Date ?? DateTime.Now.Date;

            var response = new DashboardChartsResponse
            {
                OccupiedCount = filteredRooms.Count(r => r.CurrentOccupancy >= r.Capacity),
                AvailableCount = filteredRooms.Count(r => r.CurrentOccupancy < r.Capacity),
                // Luôn hiển thị tất cả các tòa nhà trong biểu đồ, không phụ thuộc vào filter
                OccupancyByBuilding = BuildOccupancyByBuilding(rooms, buildings),
                ContractsByWeek = BuildContractTrend(contracts, dateFrom, dateTo)
            };

            return Ok(response);
        }

        [HttpGet("alerts")]
        public async Task<ActionResult<List<AlertResponse>>> GetAlerts()
        {
            var contracts = (await _contractBUS.GetAllContractsAsync()).ToDictionary(c => c.ContractID);
            var payments = await _paymentBUS.GetAllPaymentsAsync();
            var violations = await _violationBUS.GetAllViolationsAsync();

            var alerts = new List<AlertResponse>();

            var overduePayments = payments
                .Where(p => p.PaymentStatus.Equals("Late", StringComparison.OrdinalIgnoreCase) ||
                            (p.PaymentStatus != "Paid" && p.PaymentDate < DateTime.Now.AddDays(-30)))
                .OrderByDescending(p => p.PaymentDate)
                .Take(3);

            foreach (var payment in overduePayments)
            {
                if (!contracts.TryGetValue(payment.ContractID, out var contract)) continue;
                alerts.Add(new AlertResponse
                {
                    Type = "Thanh toán",
                    Message = $"Hóa đơn {payment.PaymentID} của {contract.StudentName} đang quá hạn.",
                    Date = payment.PaymentDate
                });
            }

            var pendingViolations = violations
                .Where(v => !IsClosedViolation(v.Status))
                .OrderByDescending(v => v.ViolationDate)
                .Take(3);

            foreach (var violation in pendingViolations)
            {
                alerts.Add(new AlertResponse
                {
                    Type = "Vi phạm",
                    Message = $"{violation.StudentName ?? violation.StudentID}: {violation.ViolationType}",
                    Date = violation.ViolationDate
                });
            }

            return Ok(alerts);
        }

        [HttpGet("activities")]
        public async Task<ActionResult<List<ActivityResponse>>> GetActivities([FromQuery] int limit = 20)
        {
            limit = Math.Clamp(limit, 1, 50);

            // Chạy tuần tự để tránh concurrent access trên cùng DbContext instance
            var contracts = await _contractBUS.GetAllContractsAsync();
            var payments = await _paymentBUS.GetAllPaymentsAsync();
            var violations = await _violationBUS.GetAllViolationsAsync();

            var activities = new List<ActivityResponse>();

            activities.AddRange(contracts
                .OrderByDescending(c => c.CreatedDate)
                .Take(limit)
                .Select(c => new ActivityResponse
                {
                    Time = c.CreatedDate,
                    Description = $"Hợp đồng mới: {c.StudentName} - phòng {c.RoomNumber}"
                }));

            activities.AddRange(payments
                .OrderByDescending(p => p.PaymentDate)
                .Take(limit)
                .Select(p => new ActivityResponse
                {
                    Time = p.PaymentDate,
                    Description = $"Thanh toán {p.PaymentID} - trạng thái {p.PaymentStatus}"
                }));

            activities.AddRange(violations
                .OrderByDescending(v => v.ViolationDate)
                .Take(limit)
                .Select(v => new ActivityResponse
                {
                    Time = v.ViolationDate,
                    Description = $"Vi phạm {v.ViolationType} - {v.StudentName ?? v.StudentID}"
                }));

            return Ok(activities
                .OrderByDescending(a => a.Time)
                .Take(limit)
                .ToList());
        }

        private async Task<(List<RoomReadDTO> rooms, Dictionary<string, BuildingReadDTO> buildings)> LoadRoomsAsync()
        {
            // Chạy tuần tự để tránh concurrent access trên cùng DbContext instance
            var rooms = (await _roomBUS.GetAllRoomsAsync()).ToList();
            var buildings = (await _buildingBUS.GetAllBuildingAsync())
                .ToDictionary(b => b.BuildingID, b => b, StringComparer.OrdinalIgnoreCase);

            return (rooms, buildings);
        }

        private static IEnumerable<RoomReadDTO> FilterRoomsByBuilding(
            IEnumerable<RoomReadDTO> rooms,
            IReadOnlyDictionary<string, BuildingReadDTO> buildings,
            string? filter)
        {
            if (string.IsNullOrWhiteSpace(filter) ||
                filter.StartsWith("tất cả", StringComparison.OrdinalIgnoreCase))
            {
                return rooms;
            }

            return rooms.Where(room =>
            {
                var buildingName = buildings.TryGetValue(room.BuildingID, out var dto)
                    ? dto.BuildingName
                    : room.BuildingID;

                return buildingName.Contains(filter, StringComparison.OrdinalIgnoreCase);
            });
        }

        private static List<OccupancyByBuildingItem> BuildOccupancyByBuilding(
            IEnumerable<RoomReadDTO> rooms,
            IReadOnlyDictionary<string, BuildingReadDTO> buildings)
        {
            // Tạo dictionary từ rooms đã group
            var roomsByBuilding = rooms
                .GroupBy(r => r.BuildingID)
                .ToDictionary(g => g.Key, g => g, StringComparer.OrdinalIgnoreCase);

            // Hiển thị tất cả buildings, không chỉ những building có rooms
            return buildings.Values
                .Select(building =>
                {
                    var buildingId = building.BuildingID;
                    var buildingRooms = roomsByBuilding.TryGetValue(buildingId, out var roomGroup)
                        ? roomGroup
                        : Enumerable.Empty<RoomReadDTO>();

                    return new OccupancyByBuildingItem
                    {
                        Building = building.BuildingName,
                        Occupied = buildingRooms.Sum(r => r.CurrentOccupancy),
                        Capacity = buildingRooms.Sum(r => r.Capacity)
                    };
                })
                .OrderBy(item => item.Building)
                .ToList();
        }

        private static List<ContractByWeekItem> BuildContractTrend(
            IEnumerable<ContractReadDTO> contracts,
            DateTime from,
            DateTime to)
        {
            return contracts
                .Where(c => c.CreatedDate.Date >= from && c.CreatedDate.Date <= to)
                .GroupBy(c =>
                {
                    var week = ISOWeek.GetWeekOfYear(c.CreatedDate);
                    var year = c.CreatedDate.Year;
                    return (week, year);
                })
                .OrderBy(g => g.Key.year)
                .ThenBy(g => g.Key.week)
                .Select(g => new ContractByWeekItem
                {
                    Week = $"Tuần {g.Key.week:D2}/{g.Key.year}",
                    Count = g.Count()
                })
                .ToList();
        }

        private static int EstimateFloors(int roomCount) =>
            Math.Max(1, (int)Math.Ceiling(roomCount / 20.0));

        private static bool IsPendingStatus(string status) =>
            status.Equals("Pending", StringComparison.OrdinalIgnoreCase) ||
            status.Equals("AwaitingApproval", StringComparison.OrdinalIgnoreCase);

        private static bool IsClosedViolation(string status) =>
            status.Equals("Closed", StringComparison.OrdinalIgnoreCase) ||
            status.Equals("Resolved", StringComparison.OrdinalIgnoreCase);
    }
}

