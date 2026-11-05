using CampusPlacementManagement.API.DTOs.PlacementOfficer;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.API.Services;

public class PlacementOfficerService : IPlacementOfficerService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICollegeRepository _collegeRepository;
    private readonly IPlacementOfficerRepository _officerRepository;

    public PlacementOfficerService(
        UserManager<ApplicationUser> userManager,
        ICollegeRepository collegeRepository,
        IPlacementOfficerRepository officerRepository)
    {
        _userManager = userManager;
        _collegeRepository = collegeRepository;
        _officerRepository = officerRepository;
    }

    public async Task<bool> RegisterAsync(PlacementOfficerRegisterDto dto)
    {
        var college = await _collegeRepository.GetByIdAsync(dto.CollegeId);
        if (college == null || college.Status != CollegeStatus.Approved)
        {
            throw new InvalidOperationException("Selected college is not approved or does not exist.");
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

        await _userManager.AddToRoleAsync(user, "PlacementOfficer");

        var officer = new PlacementOfficer
        {
            UserId = user.Id,
            CollegeId = dto.CollegeId,
            Designation = dto.Designation,
            Status = AccountStatus.Pending
        };

        await _officerRepository.AddAsync(officer);
        return true;
    }

    public async Task<bool> ApproveOfficerAsync(int officerId, string adminUserId)
    {
        var officer = await _officerRepository.GetByIdAsync(officerId);
        if (officer == null) return false;

        officer.Status = AccountStatus.Approved;
        officer.ApprovedBy = adminUserId;
        officer.ApprovedAt = DateTime.UtcNow;

        var user = await _userManager.FindByIdAsync(officer.UserId);
        if (user != null)
        {
            user.AccountStatus = AccountStatus.Approved;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        await _officerRepository.UpdateAsync(officer);
        return true;
    }

    public async Task<bool> RejectOfficerAsync(int officerId)
    {
        var officer = await _officerRepository.GetByIdAsync(officerId);
        if (officer == null) return false;

        officer.Status = AccountStatus.Rejected;

        var user = await _userManager.FindByIdAsync(officer.UserId);
        if (user != null)
        {
            user.AccountStatus = AccountStatus.Rejected;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        await _officerRepository.UpdateAsync(officer);
        return true;
    }
}