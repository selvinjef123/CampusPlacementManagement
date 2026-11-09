using CampusPlacementManagement.API.DTOs.JobApplication;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;

namespace CampusPlacementManagement.API.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _applicationRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IJobOpeningRepository _jobRepository;

    public JobApplicationService(
        IJobApplicationRepository applicationRepository,
        IStudentRepository studentRepository,
        IJobOpeningRepository jobRepository)
    {
        _applicationRepository = applicationRepository;
        _studentRepository = studentRepository;
        _jobRepository = jobRepository;
    }

    public async Task<JobApplicationResponseDto> ApplyForJobAsync(string studentUserId, JobApplicationApplyDto dto)
    {
        var student = await _studentRepository.GetByUserIdAsync(studentUserId);
        if (student == null || student.Status != AccountStatus.Approved)
        {
            throw new InvalidOperationException("Only approved students can apply for jobs.");
        }

        var job = await _jobRepository.GetByIdAsync(dto.JobId);
        if (job == null || job.Status != JobOpeningStatus.Active)
        {
            throw new InvalidOperationException("Job opening is not active or does not exist.");
        }

        if (job.ApplicationDeadline < DateTime.UtcNow)
        {
            throw new InvalidOperationException("The application deadline for this job has passed.");
        }

        // Target college eligibility check
        bool isTargeted = job.TargetColleges.Any(tc => tc.CollegeId == student.CollegeId);
        if (!isTargeted)
        {
            throw new InvalidOperationException("This job opening is not targeted to your college.");
        }

        // Check CGPA and Backlog eligibility
        if (student.CGPA < job.MinCGPA || student.ActiveBacklogs > job.MaxBacklogs)
        {
            throw new InvalidOperationException("You do not meet the academic eligibility criteria (CGPA/Backlogs) for this job.");
        }

        // Duplicate application check
        var existingApp = await _applicationRepository.GetByStudentAndJobAsync(student.StudentId, dto.JobId);
        if (existingApp != null)
        {
            throw new InvalidOperationException("You have already applied for this job opening.");
        }

        var application = new JobApplication
        {
            JobId = dto.JobId,
            StudentId = student.StudentId,
            Status = ApplicationStatus.Applied,
            AppliedAt = DateTime.UtcNow,
            CoverLetterUrl = dto.CoverLetterUrl
        };

        await _applicationRepository.AddAsync(application);

        // Record initial status history
        var history = new ApplicationStatusHistory
        {
            ApplicationId = application.ApplicationId,
            PreviousStatus = null,
            NewStatus = ApplicationStatus.Applied,
            UpdatedByUserId = studentUserId,
            Remarks = "Initial job application submitted.",
            UpdatedAt = DateTime.UtcNow
        };

        await _applicationRepository.AddStatusHistoryAsync(history);

        var createdApp = await _applicationRepository.GetByIdAsync(application.ApplicationId);
        return MapToDto(createdApp!);
    }

    public async Task<IEnumerable<JobApplicationResponseDto>> GetStudentApplicationsAsync(string studentUserId)
    {
        var student = await _studentRepository.GetByUserIdAsync(studentUserId);
        if (student == null) return Enumerable.Empty<JobApplicationResponseDto>();

        var apps = await _applicationRepository.GetByStudentIdAsync(student.StudentId);
        return apps.Select(MapToDto);
    }

    public async Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForJobAsync(int jobId)
    {
        var apps = await _applicationRepository.GetByJobIdAsync(jobId);
        return apps.Select(MapToDto);
    }

    public async Task<IEnumerable<JobApplicationResponseDto>> GetApplicationsForCollegeAsync(int collegeId)
    {
        var apps = await _applicationRepository.GetByCollegeIdAsync(collegeId);
        return apps.Select(MapToDto);
    }

    public async Task<IEnumerable<JobApplicationResponseDto>> GetAllApplicationsAsync()
    {
        var apps = await _applicationRepository.GetAllAsync();
        return apps.Select(MapToDto);
    }

    public async Task<JobApplicationResponseDto?> GetApplicationByIdAsync(int applicationId)
    {
        var app = await _applicationRepository.GetByIdAsync(applicationId);
        return app == null ? null : MapToDto(app);
    }

    public async Task<bool> UpdateApplicationStatusAsync(int applicationId, string updatedByUserId, JobApplicationStatusUpdateDto dto)
    {
        var app = await _applicationRepository.GetByIdAsync(applicationId);
        if (app == null) return false;

        var previousStatus = app.Status;
        app.Status = dto.NewStatus;

        await _applicationRepository.UpdateAsync(app);

        var history = new ApplicationStatusHistory
        {
            ApplicationId = app.ApplicationId,
            PreviousStatus = previousStatus,
            NewStatus = dto.NewStatus,
            UpdatedByUserId = updatedByUserId,
            Remarks = dto.Remarks,
            UpdatedAt = DateTime.UtcNow
        };

        await _applicationRepository.AddStatusHistoryAsync(history);
        return true;
    }

    private static JobApplicationResponseDto MapToDto(JobApplication ja) => new()
    {
        ApplicationId = ja.ApplicationId,
        JobId = ja.JobId,
        JobTitle = ja.JobOpening?.Title ?? string.Empty,
        CompanyName = ja.JobOpening?.Company?.CompanyName ?? string.Empty,
        StudentId = ja.StudentId,
        StudentName = ja.Student?.User?.FullName ?? string.Empty,
        RegisterNumber = ja.Student?.RegisterNumber ?? string.Empty,
        CollegeName = ja.Student?.College?.CollegeName ?? string.Empty,
        Department = ja.Student?.Department ?? string.Empty,
        CGPA = ja.Student?.CGPA ?? 0,
        Status = ja.Status,
        AppliedAt = ja.AppliedAt,
        CoverLetterUrl = ja.CoverLetterUrl,
        StatusHistories = ja.StatusHistories.Select(sh => new ApplicationStatusHistoryDto
        {
            HistoryId = sh.HistoryId,
            PreviousStatus = sh.PreviousStatus,
            NewStatus = sh.NewStatus,
            UpdatedByUserId = sh.UpdatedByUserId,
            UpdatedByUserName = sh.UpdatedByUser?.FullName ?? string.Empty,
            Remarks = sh.Remarks,
            UpdatedAt = sh.UpdatedAt
        }).OrderByDescending(sh => sh.UpdatedAt).ToList()
    };
}