using CampusPlacementManagement.API.DTOs.PlacementOfficer;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface IPlacementOfficerService
{
    Task<bool> RegisterAsync(PlacementOfficerRegisterDto dto);
    Task<bool> ApproveOfficerAsync(int officerId, string adminUserId);
    Task<bool> RejectOfficerAsync(int officerId);
    Task<PlacementOfficerResponseDto?> GetByUserIdAsync(string userId);
}