using Application.Dtos;
using Application.Dtos.RoomDto;
using Application.Interfaces.IHistoryReservationRoom;
using Application.Interfaces.IRoom;
using Application.Interfaces.IUser;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoomsService _roomsService;
        private readonly IHistoryReservationRoomService _historyReservationRoomService;

        public AdminController(
            IUserService userService,
            IRoomsService roomsService,
            IHistoryReservationRoomService historyReservationRoomService)
        {
            _userService = userService;
            _roomsService = roomsService;
            _historyReservationRoomService = historyReservationRoomService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            ViewData["ActiveMenu"] = "Pengguna";
            var viewModel = await GetUserDashboardViewModel();

            return View(viewModel);
        }

        public async Task<IActionResult> Users()
        {
            if (!IsAdminUser())
                return Unauthorized();

            var viewModel = await GetUserDashboardViewModel();
            return PartialView("_UserTabContainerPartial", viewModel);
        }

        public async Task<IActionResult> Ruangan()
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            ViewData["Title"] = "Ruangan";
            ViewData["ActiveMenu"] = "Ruangan";

            var viewModel = await GetRoomDashboardViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(AddRoomDto dto)
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Data ruangan belum lengkap."
                });
            }

            var result = await _roomsService.CreateAsync(dto);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoom(UpdateRoomDto dto)
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Data ruangan belum lengkap."
                });
            }

            var result = await _roomsService.UpdateAsync(dto);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            var result = await _roomsService.DeleteAsync(id);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUserDto dto)
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _userService.CreateAsync(dto);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _userService.UpdateAsync(dto);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            var result = await _userService.SoftDeleteAsync(id);
            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> HardDeleteUser(int id)
        {
            if (!IsAdminUser())
                return RedirectToAction("Login", "Auth");

            var result = await _userService.HardDeleteAsync(id);
            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        private async Task<UserDashboardViewModel> GetUserDashboardViewModel()
        {
            var result = await _userService.GetAllAsync();
            var users = result.Data ?? new List<UserDto>();

            return new UserDashboardViewModel
            {
                ActiveUsers = users.Where(user => !user.IsDeleted && user.Role.ToString() != "Admin").ToList(),
                DeletedUsers = users.Where(user => user.IsDeleted).ToList()
            };
        }

        private async Task<RoomDashboardViewModel> GetRoomDashboardViewModel()
        {
            var roomsResult = await _roomsService.GetAllAsync();
            var finishedReservationsResult = await _historyReservationRoomService.GetFinishedReservationsAsync();
            var runningReservationsResult = await _historyReservationRoomService.GetRunningReservationsAsync();

            return new RoomDashboardViewModel
            {
                Rooms = roomsResult.Data ?? new List<RoomDto>(),
                FinishedReservations = finishedReservationsResult.Data ?? new List<HistoryReservationRoomDto>(),
                RunningReservations = runningReservationsResult.Data ?? new List<HistoryReservationRoomDto>()
            };
        }

        private bool IsAdminUser()
        {
            var token = Request.Cookies["jwt_token"];
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var role = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

                return role == "Admin" && jwt.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }
}
