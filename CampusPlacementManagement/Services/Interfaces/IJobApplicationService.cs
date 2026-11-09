using CampusPlacementManagement.API.DTOs.JobApplication;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface IJobApplicationService
{
    Task<JobApplicationResponseDto> ApplyForJobAsync(string studentUserId, JobApplicationApplyDto dto);
    Task<IEnumerable<JobApplicationResponseDto>> GetStudentApplicationsAsync(string studentUserId);
    Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForJobAsync(int jobId);
    Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForCollegeAsync(int collegeId);
    Task<IEnumerable<JobApplicationResponseDto>> GetAllApplicationsAsync();
    Task<JobApplicationResponseDto?> GetApplicationByIdAsync(int applicationId);
    Task<bool> UpdateApplicationStatusAsync(int applicationId, string updatedByUserId, JobApplicationStatusUpdateDto dto);
}