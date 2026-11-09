using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(int id);
    Task<JobApplication?> GetByStudentAndJobAsync(int studentId, int jobId);
    Task<IEnumerable<JobApplication>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<JobApplication>> GetByJobIdAsync(int jobId);
    Task<IEnumerable<JobApplication>> GetByCollegeIdAsync(int collegeId);
    Task<IEnumerable<JobApplication>> GetAllAsync();
    Task AddAsync(JobApplication application);
    Task UpdateAsync(JobApplication application);
    Task AddStatusHistoryAsync(ApplicationStatusHistory history);
}