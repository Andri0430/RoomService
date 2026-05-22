using Application.Dtos;

namespace Web.ViewModels
{
    public class UserDashboardViewModel
    {
        public List<UserDto> ActiveUsers { get; set; } = new();
        public List<UserDto> DeletedUsers { get; set; } = new();
    }
}
