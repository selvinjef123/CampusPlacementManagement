using CampusPlacementManagement.API.DTOs.Company;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyResponseDto> RegisterCompanyAsync(CompanyRegisterDto dto);
    Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync();
    Task<IEnumerable<CompanyResponseDto>> GetPendingCompaniesAsync();
    Task<CompanyResponseDto?> GetCompanyByIdAsync(int id);
    Task<bool> UpdateCompanyStatusAsync(int id, CompanyStatus status);
}