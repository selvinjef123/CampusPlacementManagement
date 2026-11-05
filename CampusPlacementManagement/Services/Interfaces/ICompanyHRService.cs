using CampusPlacementManagement.API.DTOs.CompanyHR;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface ICompanyHRService
{
    Task<bool> RegisterAsync(CompanyHRRegisterDto dto);
    Task<IEnumerable<CompanyHRResponseDto>> GetPendingHRsAsync();
    Task<IEnumerable<CompanyHRResponseDto>> GetAllHRsAsync();
    Task<CompanyHRResponseDto?> GetByUserIdAsync(string userId);
    Task<bool> ApproveHRAsync(int hrId, string adminUserId);
    Task<bool> RejectHRAsync(int hrId);
}