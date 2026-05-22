using Application.Dtos.RoomDto;
using Application.Interfaces.IHistoryReservationRoom;
using Application.Interfaces.IRoom;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class RoomService : IRoomsService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHistoryReservationRoomRepository _historyReservationRoomRepository;
        private readonly IMapper _mapper;

        public RoomService(
            IRoomRepository roomRepository,
            IHistoryReservationRoomRepository historyReservationRoomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _historyReservationRoomRepository = historyReservationRoomRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateAsync(AddRoomDto dto)
        {
            try
            {
                var roomByName = await _roomRepository.GetByNameRoomAsync(dto.RoomName);
                if (roomByName != null)
                    return ApiResponse<string>.FailureResult($"Ruangan : {dto.RoomName} sudah pernah ditambahkan");

                var room = new Room(dto.RoomName);
                await _roomRepository.Create(room);

                return ApiResponse<string>.SuccessResult("Ruangan berhasil ditambahkan", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<RoomDto>>> GetAllAsync()
        {
            try
            {
                var rooms = await _roomRepository.GetAllAsync();
                if (!rooms.Any())
                    return ApiResponse<List<RoomDto>>.SuccessResult("Data ruangan masih kosong", new List<RoomDto>());

                var result = _mapper.Map<List<RoomDto>>(rooms);

                return ApiResponse<List<RoomDto>>.SuccessResult("Berhasil mendapatkan data", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<RoomDto>>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<RoomDto?>> GetByIdAsync(int id)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(id);
                if (room == null)
                    return ApiResponse<RoomDto?>.FailureResult("Data tidak ditemukan");

                var result = _mapper.Map<RoomDto>(room);

                return ApiResponse<RoomDto?>.SuccessResult("Berhasil mendapatkan data", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<RoomDto?>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> GetByNameRoomAsync(string room)
        {
            try
            {
                var roomByName = await _roomRepository.GetByNameRoomAsync(room);
                if (roomByName == null)
                    return ApiResponse<string>.FailureResult($"Ruangan dengan nama {room} tidak ditemukan");

                return ApiResponse<string>.SuccessResult($"Ruangan dengan nama {room} ditemukan", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdateRoomDto dto)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(dto.Id);
                if (room == null)
                    return ApiResponse<string>.FailureResult($"Ruangan dengan id {dto.Id} tidak ditemukan");

                var hasRunningReservation = await _historyReservationRoomRepository
                    .HasRunningReservationByRoomIdAsync(dto.Id);

                if (hasRunningReservation)
                    return ApiResponse<string>.FailureResult("Ruangan tidak dapat diperbarui karena masih memiliki reservasi hari ini atau mendatang.");

                var roomByName = await _roomRepository.GetByNameRoomAsync(dto.RoomName);
                if (roomByName != null && roomByName.Id != dto.Id)
                    return ApiResponse<string>.FailureResult($"Ruangan : {dto.RoomName} sudah pernah ditambahkan");

                room.Update(dto.RoomName);
                await _roomRepository.Update(room);

                return ApiResponse<string>.SuccessResult("Ruangan berhasil diperbarui", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Terjadi kesalahan: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(id);
                if (room == null)
                    return ApiResponse<string>.FailureResult($"Data Id {id} tidak ditemukan");

                var hasRunningReservation = await _historyReservationRoomRepository
                    .HasRunningReservationByRoomIdAsync(id);

                if (hasRunningReservation)
                    return ApiResponse<string>.FailureResult("Ruangan tidak dapat dihapus karena masih memiliki reservasi hari ini atau mendatang.");

                await _roomRepository.Delete(room);

                return ApiResponse<string>.SuccessResult($"Berhasil menghapus data Ruangan id : {id}", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Error message : {ex.Message}");
            }
        }
    }
}
