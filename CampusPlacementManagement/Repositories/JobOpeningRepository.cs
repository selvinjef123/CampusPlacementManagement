using CampusPlacementManagement.API.Data;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Repositories;

public class JobOpeningRepository : IJobOpeningRepository
{
    private readonly ApplicationDbContext _context;

    public JobOpeningRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobOpening?> GetByIdAsync(int id)
    {
        return await _context.JobOpenings
            .Include(j => j.Company)
            .Include(j => j.CreatedByHR)
                .ThenInclude(hr => hr.User)
            .Include(j => j.TargetColleges)
                .ThenInclude(tc => tc.College)
            .FirstOrDefaultAsync(j => j.JobId == id);
    }

    public async Task<IEnumerable<JobOpening>> GetAllAsync()
    {
        return await _context.JobOpenings
            .Include(j => j.Company)
            .Include(j => j.CreatedByHR)
                .ThenInclude(hr => hr.User)
            .Include(j => j.TargetColleges)
                .ThenInclude(tc => tc.College)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId)
    {
        return await _context.JobOpenings
            .Include(j => j.Company)
            .Include(j => j.CreatedByHR)
                .ThenInclude(hr => hr.User)
            .Include(j => j.TargetColleges)
                .ThenInclude(tc => tc.College)
            .Where(j => j.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobOpening>> GetTargetedJobsForCollegeAsync(int collegeId)
    {
        return await _context.JobOpenings
            .Include(j => j.Company)
            .Include(j => j.CreatedByHR)
                .ThenInclude(hr => hr.User)
            .Include(j => j.TargetColleges)
                .ThenInclude(tc => tc.College)
            .Where(j => j.TargetColleges.Any(tc => tc.CollegeId == collegeId))
            .ToListAsync();
    }

    public async Task AddAsync(JobOpening jobOpening)
    {
        await _context.JobOpenings.AddAsync(jobOpening);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobOpening jobOpening)
    {
        _context.JobOpenings.Update(jobOpening);
        await _context.SaveChangesAsync();
    }
}