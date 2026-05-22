using Application.Common;
using Application.Dtos.AuthDto;
using Application.Interfaces.IAuth;
using Application.Interfaces.IUser;
using Application.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (user is null)
                    return ApiResponse<LoginResponseDto>.FailureResult("Email atau password salah");

                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    return ApiResponse<LoginResponseDto>.FailureResult("Email atau password salah");

                if (user.IsDeleted)
                    return ApiResponse<LoginResponseDto>.FailureResult("Akun tidak aktif");

                var token = GenerateJwtToken(user);
                var result = new LoginResponseDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    Token = token
                };

                return ApiResponse<LoginResponseDto>.SuccessResult("Login berhasil", result);
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.FailureResult($"Terjadi kesalahan: {ex.Message}");
            }
        }

        private string GenerateJwtToken(Domain.Entities.User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
