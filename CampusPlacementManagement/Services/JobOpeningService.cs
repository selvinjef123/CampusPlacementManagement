using CampusPlacementManagement.API.DTOs.JobOpening;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;

namespace CampusPlacementManagement.API.Services;

public class JobOpeningService : IJobOpeningService
{
    private readonly IJobOpeningRepository _jobRepository;
    private readonly ICompanyHRRepository _hrRepository;
    private readonly ICollegeRepository _collegeRepository;

    public JobOpeningService(
        IJobOpeningRepository jobRepository,
        ICompanyHRRepository hrRepository,
        ICollegeRepository collegeRepository)
    {
        _jobRepository = jobRepository;
        _hrRepository = hrRepository;
        _collegeRepository = collegeRepository;
    }

    public async Task<JobOpeningResponseDto> CreateJobAsync(string hrUserId, JobOpeningCreateDto dto)
    {
        var hr = await _hrRepository.GetByUserIdAsync(hrUserId);
        if (hr == null || hr.User?.AccountStatus != AccountStatus.Approved)
        {
            throw new InvalidOperationException("Only approved Company HRs can create job openings.");
        }

        var job = new JobOpening
        {
            CompanyId = hr.CompanyId,
            CreatedByHRId = hr.CompanyHRId,
            Title = dto.Title,
            Description = dto.Description,
            Requirements = dto.Requirements,
            Location = dto.Location,
            PackageLPA = dto.PackageLPA,
            MinCGPA = dto.MinCGPA,
            MaxBacklogs = dto.MaxBacklogs,
            ApplicationDeadline = dto.ApplicationDeadline,
            Status = JobOpeningStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var collegeId in dto.TargetCollegeIds)
        {
            var college = await _collegeRepository.GetByIdAsync(collegeId);
            if (college != null && college.Status == CollegeStatus.Approved)
            {
                job.TargetColleges.Add(new JobTargetCollege
                {
                    CollegeId = collegeId
                });
            }
        }

        await _jobRepository.AddAsync(job);

        // Fetch re-populated entity with all inclusions
        var createdJob = await _jobRepository.GetByIdAsync(job.JobId);
        return MapToDto(createdJob!);
    }

    public async Task<IEnumerable<JobOpeningResponseDto>> GetAllJobsAsync()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(MapToDto);
    }

    public async Task<IEnumerable<JobOpeningResponseDto>> GetJobsForCompanyAsync(int companyId)
    {
        var jobs = await _jobRepository.GetByCompanyIdAsync(companyId);
        return jobs.Select(MapToDto);
    }

    public async Task<IEnumerable<JobOpeningResponseDto>> GetTargetedJobsForCollegeAsync(int collegeId)
    {
        var jobs = await _jobRepository.GetTargetedJobsForCollegeAsync(collegeId);
        return jobs.Select(MapToDto);
    }

    public async Task<JobOpeningResponseDto?> GetJobByIdAsync(int id)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        return job == null ? null : MapToDto(job);
    }

    public async Task<bool> UpdateJobStatusAsync(int id, JobOpeningStatus status)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        if (job == null) return false;

        job.Status = status;
        await _jobRepository.UpdateAsync(job);
        return true;
    }

    private static JobOpeningResponseDto MapToDto(JobOpening j) => new()
    {
        JobId = j.JobId,
        CompanyId = j.CompanyId,
        CompanyName = j.Company?.CompanyName ?? string.Empty,
        CreatedByHRId = j.CreatedByHRId,
        HRName = j.CreatedByHR?.User?.FullName ?? string.Empty,
        Title = j.Title,
        Description = j.Description,
        Requirements = j.Requirements,
        Location = j.Location,
        PackageLPA = j.PackageLPA,
        MinCGPA = j.MinCGPA,
        MaxBacklogs = j.MaxBacklogs,
        ApplicationDeadline = j.ApplicationDeadline,
        Status = j.Status,
        CreatedAt = j.CreatedAt,
        TargetColleges = j.TargetColleges.Select(tc => new TargetCollegeDto
        {
            CollegeId = tc.CollegeId,
            CollegeName = tc.College?.CollegeName ?? string.Empty,
            CollegeCode = tc.College?.CollegeCode ?? string.Empty
        }).ToList()
    };
}