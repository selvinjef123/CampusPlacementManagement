using CampusPlacementManagement.API.DTOs.Auth;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Helpers;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtHelper _jwtHelper;

    public AuthService(UserManager<ApplicationUser> userManager, JwtHelper jwtHelper)
    {
        _userManager = userManager;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (user.AccountStatus != AccountStatus.Approved)
        {
            throw new InvalidOperationException($"Account access restricted. Current account status: {user.AccountStatus}");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? "Student";

        var (token, expiration) = _jwtHelper.GenerateJwtToken(user, primaryRole);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Role = primaryRole,
            AccountStatus = user.AccountStatus.ToString(),
            Expiration = expiration
        };
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        return result.Succeeded;
    }
}