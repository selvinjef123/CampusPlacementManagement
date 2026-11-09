using CampusPlacementManagement.API.Data;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Repositories;

public class RecruitmentRepository : IRecruitmentRepository
{
    private readonly ApplicationDbContext _context;

    public RecruitmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecruitmentDrive?> GetDriveByIdAsync(int driveId)
    {
        return await _context.RecruitmentDrives
            .Include(rd => rd.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(rd => rd.College)
            .FirstOrDefaultAsync(rd => rd.DriveId == driveId);
    }

    public async Task<IEnumerable<RecruitmentDrive>> GetDrivesByCollegeAsync(int collegeId)
    {
        return await _context.RecruitmentDrives
            .Include(rd => rd.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(rd => rd.College)
            .Where(rd => rd.CollegeId == collegeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<RecruitmentDrive>> GetDrivesByJobAsync(int jobId)
    {
        return await _context.RecruitmentDrives
            .Include(rd => rd.JobOpening)
                .ThenInclude(j => j.Company)
            .Include(rd => rd.College)
            .Where(rd => rd.JobId == jobId)
            .ToListAsync();
    }

    public async Task AddDriveAsync(RecruitmentDrive drive)
    {
        await _context.RecruitmentDrives.AddAsync(drive);
        await _context.SaveChangesAsync();
    }

    public async Task<InterviewRound?> GetRoundByIdAsync(int roundId)
    {
        return await _context.InterviewRounds
            .Include(ir => ir.JobOpening)
            .FirstOrDefaultAsync(ir => ir.InterviewRoundId == roundId);
    }

    public async Task<IEnumerable<InterviewRound>> GetRoundsByJobAsync(int jobId)
    {
        return await _context.InterviewRounds
            .Where(ir => ir.JobId == jobId)
            .OrderBy(ir => ir.RoundNumber)
            .ToListAsync();
    }

    public async Task AddRoundAsync(InterviewRound round)
    {
        await _context.InterviewRounds.AddAsync(round);
        await _context.SaveChangesAsync();
    }

    public async Task<InterviewSchedule?> GetScheduleByIdAsync(int scheduleId)
    {
        return await _context.InterviewSchedules
            .Include(isch => isch.InterviewRound)
            .Include(isch => isch.JobApplication)
                .ThenInclude(ja => ja.JobOpening)
                    .ThenInclude(j => j.Company)
            .Include(isch => isch.JobApplication)
                .ThenInclude(ja => ja.Student)
                    .ThenInclude(s => s.User)
            .FirstOrDefaultAsync(isch => isch.ScheduleId == scheduleId);
    }

    public async Task<IEnumerable<InterviewSchedule>> GetSchedulesByApplicationAsync(int applicationId)
    {
        return await _context.InterviewSchedules
            .Include(isch => isch.InterviewRound)
            .Include(isch => isch.JobApplication)
                .ThenInclude(ja => ja.JobOpening)
                    .ThenInclude(j => j.Company)
            .Include(isch => isch.JobApplication)
                .ThenInclude(ja => ja.Student)
                    .ThenInclude(s => s.User)
            .Where(isch => isch.ApplicationId == applicationId)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterviewSchedule>> GetSchedulesByStudentUserAsync(string studentUserId)
    {
        return await _context.InterviewSchedules
            .Include(isch => isch.InterviewRound)
            .Include(isch => isch.JobApplication)
                .ThenInclude(ja => ja.JobOpening)
                    .ThenInclude(j => j.Company)
            .Include(isch => isch.JobApplication)
                .ThenInclude(ja => ja.Student)
                    .ThenInclude(s => s.User)
            .Where(isch => isch.JobApplication.Student.UserId == studentUserId)
            .OrderBy(isch => isch.ScheduledTime)
            .ToListAsync();
    }

    public async Task AddScheduleAsync(InterviewSchedule schedule)
    {
        await _context.InterviewSchedules.AddAsync(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateScheduleAsync(InterviewSchedule schedule)
    {
        _context.InterviewSchedules.Update(schedule);
        await _context.SaveChangesAsync();
    }
}