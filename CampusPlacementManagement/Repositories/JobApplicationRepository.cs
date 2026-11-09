using CampusPlacementManagement.API.Data;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Repositories;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public JobApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobApplication?> GetByIdAsync(int id)
    {
        return await _context.JobApplications
            .Include(ja => ja.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.College)
            .Include(ja => ja.StatusHistories)
                .ThenInclude(sh => sh.UpdatedByUser)
            .FirstOrDefaultAsync(ja => ja.ApplicationId == id);
    }

    public async Task<JobApplication?> GetByStudentAndJobAsync(int studentId, int jobId)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(ja => ja.StudentId == studentId && ja.JobId == jobId);
    }

    public async Task<IEnumerable<JobApplication>> GetByStudentIdAsync(int studentId)
    {
        return await _context.JobApplications
            .Include(ja => ja.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.College)
            .Include(ja => ja.StatusHistories)
                .ThenInclude(sh => sh.UpdatedByUser)
            .Where(ja => ja.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByJobIdAsync(int jobId)
    {
        return await _context.JobApplications
            .Include(ja => ja.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.College)
            .Include(ja => ja.StatusHistories)
                .ThenInclude(sh => sh.UpdatedByUser)
            .Where(ja => ja.JobId == jobId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetByCollegeIdAsync(int collegeId)
    {
        return await _context.JobApplications
            .Include(ja => ja.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.College)
            .Include(ja => ja.StatusHistories)
                .ThenInclude(sh => sh.UpdatedByUser)
            .Where(ja => ja.Student.CollegeId == collegeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        return await _context.JobApplications
            .Include(ja => ja.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.User)
            .Include(ja => ja.Student)
                .ThenInclude(s => s.College)
            .Include(ja => ja.StatusHistories)
                .ThenInclude(sh => sh.UpdatedByUser)
            .ToListAsync();
    }

    public async Task AddAsync(JobApplication application)
    {
        await _context.JobApplications.AddAsync(application);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobApplication application)
    {
        _context.JobApplications.Update(application);
        await _context.SaveChangesAsync();
    }

    public async Task AddStatusHistoryAsync(ApplicationStatusHistory history)
    {
        await _context.ApplicationStatusHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }
}