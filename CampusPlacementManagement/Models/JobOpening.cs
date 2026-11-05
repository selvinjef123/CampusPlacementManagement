using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Models;

public class JobOpening
{
    public int JobId { get; set; }
    public int CompanyId { get; set; }
    public int CreatedByHRId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal? SalaryPackage { get; set; }
    public decimal? MinimumCGPA { get; set; }
    public int? MaximumBacklogs { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public bool IsOpenToAllColleges { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual Company Company { get; set; } = null!;
    public virtual CompanyHR CreatedByHR { get; set; } = null!;
    public virtual ICollection<JobTargetCollege> TargetColleges { get; set; } = new List<JobTargetCollege>();
    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    public virtual ICollection<RecruitmentDrive> RecruitmentDrives { get; set; } = new List<RecruitmentDrive>();
    public virtual ICollection<InterviewRound> InterviewRounds { get; set; } = new List<InterviewRound>();
}