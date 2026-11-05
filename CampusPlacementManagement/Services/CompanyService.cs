using CampusPlacementManagement.API.DTOs.Company;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;

namespace CampusPlacementManagement.API.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<CompanyResponseDto> RegisterCompanyAsync(CompanyRegisterDto dto)
    {
        if (await _companyRepository.CompanyEmailExistsAsync(dto.CompanyEmail))
        {
            throw new InvalidOperationException($"Company with email '{dto.CompanyEmail}' is already registered.");
        }

        var company = new Company
        {
            CompanyName = dto.CompanyName,
            CompanyEmail = dto.CompanyEmail,
            Website = dto.Website,
            Industry = dto.Industry,
            Address = dto.Address,
            Description = dto.Description,
            Status = CompanyStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _companyRepository.AddAsync(company);
        return MapToDto(company);
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync()
    {
        var companies = await _companyRepository.GetAllAsync();
        return companies.Select(MapToDto);
    }

    public async Task<IEnumerable<CompanyResponseDto>> GetPendingCompaniesAsync()
    {
        var companies = await _companyRepository.GetPendingCompaniesAsync();
        return companies.Select(MapToDto);
    }

    public async Task<CompanyResponseDto?> GetCompanyByIdAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        return company == null ? null : MapToDto(company);
    }

    public async Task<bool> UpdateCompanyStatusAsync(int id, CompanyStatus status)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null) return false;

        company.Status = status;
        await _companyRepository.UpdateAsync(company);
        return true;
    }

    private static CompanyResponseDto MapToDto(Company c) => new()
    {
        CompanyId = c.CompanyId,
        CompanyName = c.CompanyName,
        CompanyEmail = c.CompanyEmail,
        Website = c.Website,
        Industry = c.Industry,
        Address = c.Address,
        Description = c.Description,
        Status = c.Status,
        CreatedAt = c.CreatedAt
    };
}