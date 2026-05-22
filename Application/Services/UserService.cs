using Application.Dtos;
using Application.Interfaces.IUser;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateAsync(AddUserDto dto)
        {
            try
            {
                var userByEmail = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (userByEmail != null)
                    return ApiResponse<string>.FailureResult($"Pengguna dengan email : {dto.Email} sudah terdaftar");

                var userByPhone = await _userRepository.GetUserByPhoneNumber(dto.PhoneNumber);
                if (userByPhone != null)
                    return ApiResponse<string>.FailureResult($"Pengguna dengan Nomor HP : {dto.PhoneNumber} sudah terdaftar");

                var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                var user = new User(dto.Name, dto.Email, dto.PhoneNumber, hashPassword, dto.Role);

                await _userRepository.Create(user);

                return ApiResponse<string>.SuccessResult("Pengguna berhasil ditambahkan", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                if (!users.Any())
                    return ApiResponse<List<UserDto>>.SuccessResult("Data pengguna masih kosong", new List<UserDto>());

                var result = _mapper.Map<List<UserDto>>(users);

                return ApiResponse<List<UserDto>>.SuccessResult("Berhasil mendapatkan data", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserDto>>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDto?>> GetByIdAsync(int id)
        {
            try
            {
                var getUser = await _userRepository.GetByIdAsync(id);
                if (getUser == null) return ApiResponse<UserDto?>.FailureResult("Data tidak ditemukan");

                var result = _mapper.Map<UserDto>(getUser);

                return ApiResponse<UserDto?>.SuccessResult("Berhasil mendapatkan data", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto?>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> HardDeleteAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null) return ApiResponse<string>.FailureResult($"Data Id {id} tidak ditemukan");

                await _userRepository.HardDelete(user);

                return ApiResponse<string>.SuccessResult($"Berhasil menghapus permanen data pengguna : {user.Name}", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> SoftDeleteAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null) return ApiResponse<string>.FailureResult($"Data Id {id} tidak ditemukan");

                await _userRepository.SoftDelete(user);

                return ApiResponse<string>.SuccessResult($"Berhasil menghapus data pengguna id : {id}", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Error message : {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdateUserDto dto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(dto.Id);
                if (user is null)
                    return ApiResponse<string>.FailureResult($"Pengguna dengan id {dto.Id} tidak ditemukan");

                var userByEmail = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (userByEmail != null && userByEmail.Id != dto.Id)
                    return ApiResponse<string>.FailureResult($"Pengguna dengan email : {dto.Email} sudah terdaftar");

                var userByPhone = await _userRepository.GetUserByPhoneNumber(dto.PhoneNumber);
                if (userByPhone != null && userByPhone.Id != dto.Id)
                    return ApiResponse<string>.FailureResult($"Pengguna dengan Nomor HP : {dto.PhoneNumber} sudah terdaftar");

                var password = string.IsNullOrWhiteSpace(dto.Password)
                    ? null
                    : BCrypt.Net.BCrypt.HashPassword(dto.Password);

                user.Update(dto.Name, dto.Email, dto.PhoneNumber, password, dto.Role, dto.IsDeleted);
                await _userRepository.Update(user);

                return ApiResponse<string>.SuccessResult("Pengguna berhasil diperbarui", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Terjadi kesalahan: {ex.Message}");
            }
        }
    }
}
