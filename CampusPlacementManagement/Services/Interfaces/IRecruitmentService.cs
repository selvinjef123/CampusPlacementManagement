using CampusPlacementManagement.API.DTOs.RecruitmentDrive;
using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface IRecruitmentService
{
    Task<RecruitmentDrive> CreateDriveAsync(RecruitmentDriveCreateDto dto);
    Task<IEnumerable<RecruitmentDrive>> GetDrivesByCollegeAsync(int collegeId);
    Task<InterviewRound> CreateRoundAsync(InterviewRoundCreateDto dto);
    Task<IEnumerable<InterviewRound>> GetRoundsByJobAsync(int jobId);
    Task<InterviewScheduleResponseDto> ScheduleInterviewAsync(InterviewScheduleCreateDto dto);
    Task<IEnumerable<InterviewScheduleResponseDto>> GetStudentSchedulesAsync(string studentUserId);
}