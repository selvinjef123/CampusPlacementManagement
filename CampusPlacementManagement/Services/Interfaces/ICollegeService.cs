using CampusPlacementManagement.API.DTOs.College;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface ICollegeService
{
    Task<CollegeResponseDto> RegisterCollegeAsync(CollegeRegisterDto dto);
    Task<IEnumerable<CollegeResponseDto>> GetAllCollegesAsync();
    Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync();
    Task<CollegeResponseDto?> GetCollegeByIdAsync(int id);
    Task<bool> UpdateCollegeStatusAsync(int id, CollegeStatus status);
}