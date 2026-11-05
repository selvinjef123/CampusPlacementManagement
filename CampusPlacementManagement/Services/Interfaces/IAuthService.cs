using CampusPlacementManagement.API.DTOs.Auth;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto);
}