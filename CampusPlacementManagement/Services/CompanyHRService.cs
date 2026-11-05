using CampusPlacementManagement.API.DTOs.CompanyHR;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.API.Services;

public class CompanyHRService : ICompanyHRService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyHRRepository _hrRepository;

    public CompanyHRService(
        UserManager<ApplicationUser> userManager,
        ICompanyRepository companyRepository,
        ICompanyHRRepository hrRepository)
    {
        _userManager = userManager;
        _companyRepository = companyRepository;
        _hrRepository = hrRepository;
    }

    public async Task<bool> RegisterAsync(CompanyHRRegisterDto dto)
    {
        var company = await _companyRepository.GetByIdAsync(dto.CompanyId);
        if (company == null || company.Status != CompanyStatus.Approved)
        {
            throw new InvalidOperationException("Selected company is not approved or does not exist.");
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with email '{dto.Email}' already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            FullName = dto.FullName,
            AccountStatus = AccountStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return false;

        await _userManager.AddToRoleAsync(user, "CompanyHR");

        var hr = new CompanyHR
        {
            UserId = user.Id,
            CompanyId = dto.CompanyId,
            Designation = dto.Designation
        };

        await _hrRepository.AddAsync(hr);
        return true;
    }

    public async Task<IEnumerable<CompanyHRResponseDto>> GetPendingHRsAsync()
    {
        var hrs = await _hrRepository.GetPendingHRsAsync();
        return hrs.Select(MapToDto);
    }

    public async Task<IEnumerable<CompanyHRResponseDto>> GetAllHRsAsync()
    {
        var hrs = await _hrRepository.GetAllAsync();
        return hrs.Select(MapToDto);
    }

    public async Task<CompanyHRResponseDto?> GetByUserIdAsync(string userId)
    {
        var hr = await _hrRepository.GetByUserIdAsync(userId);
        return hr == null ? null : MapToDto(hr);
    }

    public async Task<bool> ApproveHRAsync(int hrId, string adminUserId)
    {
        var hr = await _hrRepository.GetByIdAsync(hrId);
        if (hr == null) return false;

        hr.ApprovedBy = adminUserId;
        hr.ApprovedAt = DateTime.UtcNow;

        var user = await _userManager.FindByIdAsync(hr.UserId);
        if (user != null)
        {
            user.AccountStatus = AccountStatus.Approved;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        await _hrRepository.UpdateAsync(hr);
        return true;
    }

    public async Task<bool> RejectHRAsync(int hrId)
    {
        var hr = await _hrRepository.GetByIdAsync(hrId);
        if (hr == null) return false;

        var user = await _userManager.FindByIdAsync(hr.UserId);
        if (user != null)
        {
            user.AccountStatus = AccountStatus.Rejected;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        await _hrRepository.UpdateAsync(hr);
        return true;
    }

    private static CompanyHRResponseDto MapToDto(CompanyHR hr) => new()
    {
        CompanyHRId = hr.CompanyHRId,
        UserId = hr.UserId,
        FullName = hr.User?.FullName ?? string.Empty,
        Email = hr.User?.Email ?? string.Empty,
        PhoneNumber = hr.User?.PhoneNumber ?? string.Empty,
        CompanyId = hr.CompanyId,
        CompanyName = hr.Company?.CompanyName ?? string.Empty,
        Designation = hr.Designation,
        Status = hr.User?.AccountStatus ?? AccountStatus.Pending,
        ApprovedBy = hr.ApprovedBy,
        ApprovedAt = hr.ApprovedAt
    };
}