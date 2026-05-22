using Application.Dtos;
using Application.Dtos.RoomDto;
using Application.Interfaces.IHistoryReservationRoom;
using Application.Interfaces.IRoom;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.ViewModels;

namespace Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly IRoomsService _roomsService;
        private readonly IHistoryReservationRoomService _historyReservationRoomService;

        public StaffController(
            IRoomsService roomsService,
            IHistoryReservationRoomService historyReservationRoomService)
        {
            _roomsService = roomsService;
            _historyReservationRoomService = historyReservationRoomService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsStaffUser())
                return RedirectToAction("Login", "Auth");

            var viewModel = await GetStaffViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(AddHistoryReservationRoomDto dto)
        {
            if (!IsStaffUser())
                return Unauthorized();

            var staffEmail = GetLoggedInEmail();
            if (string.IsNullOrWhiteSpace(staffEmail))
            {
                return Json(new
                {
                    success = false,
                    message = "Email staff tidak ditemukan. Silakan login ulang."
                });
            }

            dto.UserEmail = staffEmail;
            var result = await _historyReservationRoomService.CreateAsync(dto);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        private bool IsStaffUser()
        {
            var token = Request.Cookies["jwt_token"];
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var role = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

                return role == "Staff" && jwt.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }

        private string? GetLoggedInEmail()
        {
            var token = Request.Cookies["jwt_token"];
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task<StaffDashboardViewModel> GetStaffViewModel()
        {
            var rooms = await _roomsService.GetAllAsync();
            var historyRoomsByEmail =
                await _historyReservationRoomService
                    .GetHistoryReservationRoomByEmail(GetLoggedInEmail() ?? string.Empty);

            return new StaffDashboardViewModel
            {
                Rooms = rooms.Data,
                HistoryReservationRooms = historyRoomsByEmail.Data ?? new List<HistoryReservationRoomDto>()
            };
        }
    }
}
