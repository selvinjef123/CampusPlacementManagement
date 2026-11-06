using CampusPlacementManagement.API.DTOs.JobOpening;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface IJobOpeningService
{
    Task<JobOpeningResponseDto> CreateJobAsync(string hrUserId, JobOpeningCreateDto dto);
    Task<IEnumerable<JobOpeningResponseDto>> GetAllJobsAsync();
    Task<IEnumerable<JobOpeningResponseDto>> GetJobsForCompanyAsync(int companyId);
    Task<IEnumerable<JobOpeningResponseDto>> GetTargetedJobsForCollegeAsync(int collegeId);
    Task<JobOpeningResponseDto?> GetJobByIdAsync(int id);
    Task<bool> UpdateJobStatusAsync(int id, JobOpeningStatus status);
}