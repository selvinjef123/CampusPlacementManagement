using CampusPlacementManagement.API.DTOs.PlacementOfficer;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface IPlacementOfficerService
{
    Task<bool> RegisterAsync(PlacementOfficerRegisterDto dto);
    Task<bool> ApproveOfficerAsync(int officerId, string adminUserId);
    Task<bool> RejectOfficerAsync(int officerId);
}