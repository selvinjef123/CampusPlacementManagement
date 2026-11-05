using CampusPlacementManagement.API.DTOs.College;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;

namespace CampusPlacementManagement.API.Services;

public class CollegeService : ICollegeService
{
    private readonly ICollegeRepository _collegeRepository;

    public CollegeService(ICollegeRepository collegeRepository)
    {
        _collegeRepository = collegeRepository;
    }

    public async Task<CollegeResponseDto> RegisterCollegeAsync(CollegeRegisterDto dto)
    {
        if (await _collegeRepository.CollegeCodeExistsAsync(dto.CollegeCode))
        {
            throw new InvalidOperationException($"College code '{dto.CollegeCode}' already exists.");
        }

        var college = new College
        {
            CollegeName = dto.CollegeName,
            CollegeCode = dto.CollegeCode,
            UniversityName = dto.UniversityName,
            OfficialEmail = dto.OfficialEmail,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Website = dto.Website,
            Status = CollegeStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _collegeRepository.AddAsync(college);
        return MapToDto(college);
    }

    public async Task<IEnumerable<CollegeResponseDto>> GetAllCollegesAsync()
    {
        var colleges = await _collegeRepository.GetAllAsync();
        return colleges.Select(MapToDto);
    }

    public async Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync()
    {
        var colleges = await _collegeRepository.GetPendingCollegesAsync();
        return colleges.Select(MapToDto);
    }

    public async Task<CollegeResponseDto?> GetCollegeByIdAsync(int id)
    {
        var college = await _collegeRepository.GetByIdAsync(id);
        return college == null ? null : MapToDto(college);
    }

    public async Task<bool> UpdateCollegeStatusAsync(int id, CollegeStatus status)
    {
        var college = await _collegeRepository.GetByIdAsync(id);
        if (college == null) return false;

        college.Status = status;
        await _collegeRepository.UpdateAsync(college);
        return true;
    }

    private static CollegeResponseDto MapToDto(College c) => new()
    {
        CollegeId = c.CollegeId,
        CollegeName = c.CollegeName,
        CollegeCode = c.CollegeCode,
        UniversityName = c.UniversityName,
        OfficialEmail = c.OfficialEmail,
        PhoneNumber = c.PhoneNumber,
        Address = c.Address,
        Website = c.Website,
        Status = c.Status,
        CreatedAt = c.CreatedAt
    };
}