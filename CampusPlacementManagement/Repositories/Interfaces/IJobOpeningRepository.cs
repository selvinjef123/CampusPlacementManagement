using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface IJobOpeningRepository
{
    Task<JobOpening?> GetByIdAsync(int id);
    Task<IEnumerable<JobOpening>> GetAllAsync();
    Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId);
    Task<IEnumerable<JobOpening>> GetTargetedJobsForCollegeAsync(int collegeId);
    Task AddAsync(JobOpening jobOpening);
    Task UpdateAsync(JobOpening jobOpening);
}