using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface IRecruitmentRepository
{
    Task<RecruitmentDrive?> GetDriveByIdAsync(int driveId);
    Task<IEnumerable<RecruitmentDrive>> GetDrivesByCollegeAsync(int collegeId);
    Task<IEnumerable<RecruitmentDrive>> GetDrivesByJobAsync(int jobId);
    Task AddDriveAsync(RecruitmentDrive drive);

    Task<InterviewRound?> GetRoundByIdAsync(int roundId);
    Task<IEnumerable<InterviewRound>> GetRoundsByJobAsync(int jobId);
    Task AddRoundAsync(InterviewRound round);

    Task<InterviewSchedule?> GetScheduleByIdAsync(int scheduleId);
    Task<IEnumerable<InterviewSchedule>> GetSchedulesByApplicationAsync(int applicationId);
    Task<IEnumerable<InterviewSchedule>> GetSchedulesByStudentUserAsync(string studentUserId);
    Task AddScheduleAsync(InterviewSchedule schedule);
    Task UpdateScheduleAsync(InterviewSchedule schedule);
}