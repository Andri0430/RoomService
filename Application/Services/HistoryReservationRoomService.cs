using Application.Dtos;
using Application.Interfaces.IHistoryReservationRoom;
using Application.Interfaces.IRoom;
using Application.Models;
using Domain.Entities;

namespace Application.Services
{
    public class HistoryReservationRoomService : IHistoryReservationRoomService
    {
        private readonly IHistoryReservationRoomRepository _historyReservationRoomRepository;
        private readonly IRoomRepository _roomRepository;

        public HistoryReservationRoomService(
            IHistoryReservationRoomRepository historyReservationRoomRepository,
            IRoomRepository roomRepository)
        {
            _historyReservationRoomRepository = historyReservationRoomRepository;
            _roomRepository = roomRepository;
        }

        public async Task<ApiResponse<string>> CreateAsync(AddHistoryReservationRoomDto dto)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(dto.RoomId);
                if (room == null)
                    return ApiResponse<string>.FailureResult("Ruangan tidak ditemukan.");

                if (dto.BookedDateReservation.Date < DateTime.Today)
                    return ApiResponse<string>.FailureResult("Tanggal booking tidak boleh sebelum hari ini.");

                var isBooked = await _historyReservationRoomRepository
                    .IsRoomBookedOnDateAsync(dto.RoomId, dto.BookedDateReservation);

                if (isBooked)
                    return ApiResponse<string>.FailureResult("Ruangan sudah pernah di-booking pada tanggal tersebut.");

                var reservation = new HistoryReservationRoom(
                    dto.RoomId,
                    dto.UserEmail,
                    dto.BookedDateReservation.Date);

                await _historyReservationRoomRepository.Create(reservation);

                return ApiResponse<string>.SuccessResult("Reservasi ruangan berhasil dibuat.", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<HistoryReservationRoomDto>>> GetFinishedReservationsAsync()
        {
            try
            {
                var reservations = await GetReservationDtosAsync();
                var finishedReservations = reservations
                    .Where(reservation => reservation.BookedDateReservation.Date < DateTime.Today)
                    .ToList();

                return ApiResponse<List<HistoryReservationRoomDto>>.SuccessResult(
                    "Berhasil mendapatkan history selesai reservasi",
                    finishedReservations);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<HistoryReservationRoomDto>>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<HistoryReservationRoomDto>>> GetHistoryReservationRoomByEmail(string email)
        {
            try
            {
                var reservations = await _historyReservationRoomRepository.GetHistoryReservationRoomByEmail(email);

                var reservationByEmail = reservations.Select(reservation => new HistoryReservationRoomDto
                {
                    Id = reservation.Id,
                    RoomId = reservation.RoomId,
                    RoomName = reservation.Room?.RoomName ?? "-",
                    UserEmail = reservation.UserEmail,
                    BookedDateReservation = reservation.BookedDateReservation
                }).ToList();

                return ApiResponse<List<HistoryReservationRoomDto>>.SuccessResult(
                    "Berhasil mendapatkan history selesai reservasi",
                    reservationByEmail);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<HistoryReservationRoomDto>>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<HistoryReservationRoomDto>>> GetRunningReservationsAsync()
        {
            try
            {
                var reservations = await GetReservationDtosAsync();
                var runningReservations = reservations
                    .Where(reservation => reservation.BookedDateReservation.Date >= DateTime.Today)
                    .ToList();

                return ApiResponse<List<HistoryReservationRoomDto>>.SuccessResult(
                    "Berhasil mendapatkan reservasi yang sedang berjalan",
                    runningReservations);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<HistoryReservationRoomDto>>.FailureResult($"Error message : {ex.Message}");
            }
        }

        private async Task<List<HistoryReservationRoomDto>> GetReservationDtosAsync()
        {
            var reservations = await _historyReservationRoomRepository.GetAllAsync();

            return reservations
                .Select(reservation => new HistoryReservationRoomDto
                {
                    Id = reservation.Id,
                    RoomId = reservation.RoomId,
                    RoomName = reservation.Room?.RoomName ?? "-",
                    UserEmail = reservation.UserEmail,
                    BookedDateReservation = reservation.BookedDateReservation
                })
                .ToList();
        }
    }
}
