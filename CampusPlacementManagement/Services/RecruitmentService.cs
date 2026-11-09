using CampusPlacementManagement.API.DTOs.RecruitmentDrive;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;

namespace CampusPlacementManagement.API.Services;

public class RecruitmentService : IRecruitmentService
{
    private readonly IRecruitmentRepository _recruitmentRepository;
    private readonly IJobApplicationRepository _applicationRepository;

    public RecruitmentService(
        IRecruitmentRepository recruitmentRepository,
        IJobApplicationRepository applicationRepository)
    {
        _recruitmentRepository = recruitmentRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<RecruitmentDrive> CreateDriveAsync(RecruitmentDriveCreateDto dto)
    {
        var drive = new RecruitmentDrive
        {
            JobId = dto.JobId,
            CollegeId = dto.CollegeId,
            DriveName = dto.DriveName,
            DriveDate = dto.DriveDate,
            Venue = dto.Venue,
            Remarks = dto.Remarks
        };

        await _recruitmentRepository.AddDriveAsync(drive);
        return drive;
    }

    public async Task<IEnumerable<RecruitmentDrive>> GetDrivesByCollegeAsync(int collegeId)
    {
        return await _recruitmentRepository.GetDrivesByCollegeAsync(collegeId);
    }

    public async Task<InterviewRound> CreateRoundAsync(InterviewRoundCreateDto dto)
    {
        var round = new InterviewRound
        {
            JobId = dto.JobId,
            RoundNumber = dto.RoundNumber,
            RoundName = dto.RoundName,
            Description = dto.Description
        };

        await _recruitmentRepository.AddRoundAsync(round);
        return round;
    }

    public async Task<IEnumerable<InterviewRound>> GetRoundsByJobAsync(int jobId)
    {
        return await _recruitmentRepository.GetRoundsByJobAsync(jobId);
    }

    public async Task<InterviewScheduleResponseDto> ScheduleInterviewAsync(InterviewScheduleCreateDto dto)
    {
        var application = await _applicationRepository.GetByIdAsync(dto.ApplicationId);
        if (application == null)
        {
            throw new InvalidOperationException("Job Application not found.");
        }

        var round = await _recruitmentRepository.GetRoundByIdAsync(dto.InterviewRoundId);
        if (round == null)
        {
            throw new InvalidOperationException("Interview round not found.");
        }

        var schedule = new InterviewSchedule
        {
            ApplicationId = dto.ApplicationId,
            InterviewRoundId = dto.InterviewRoundId,
            ScheduledTime = dto.ScheduledTime,
            LocationOrLink = dto.LocationOrLink,
            Instructions = dto.Instructions,
            Status = "Scheduled"
        };

        await _recruitmentRepository.AddScheduleAsync(schedule);

        var createdSchedule = await _recruitmentRepository.GetScheduleByIdAsync(schedule.ScheduleId);
        return MapToDto(createdSchedule!);
    }

    public async Task<IEnumerable<InterviewScheduleResponseDto>> GetStudentSchedulesAsync(string studentUserId)
    {
        var schedules = await _recruitmentRepository.GetSchedulesByStudentUserAsync(studentUserId);
        return schedules.Select(MapToDto);
    }

    private static InterviewScheduleResponseDto MapToDto(InterviewSchedule s) => new()
    {
        ScheduleId = s.ScheduleId,
        ApplicationId = s.ApplicationId,
        StudentName = s.JobApplication?.Student?.User?.FullName ?? string.Empty,
        RegisterNumber = s.JobApplication?.Student?.RegisterNumber ?? string.Empty,
        JobTitle = s.JobApplication?.JobOpening?.Title ?? string.Empty,
        CompanyName = s.JobApplication?.JobOpening?.Company?.CompanyName ?? string.Empty,
        InterviewRoundId = s.InterviewRoundId,
        RoundNumber = s.InterviewRound?.RoundNumber ?? 0,
        RoundName = s.InterviewRound?.RoundName ?? string.Empty,
        ScheduledTime = s.ScheduledTime,
        LocationOrLink = s.LocationOrLink,
        Instructions = s.Instructions,
        Status = s.Status
    };
}