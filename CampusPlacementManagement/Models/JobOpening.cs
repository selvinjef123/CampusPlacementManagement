using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Models;

public class JobOpening
{
    [Key]
    public int JobId { get; set; }

    public int CompanyId { get; set; }
    public int CreatedByHRId { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Requirements { get; set; }

    [Required, StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PackageLPA { get; set; }

    [Column(TypeName = "decimal(3,2)")]
    public decimal MinCGPA { get; set; }

    public int MaxBacklogs { get; set; }

    public DateTime ApplicationDeadline { get; set; }

    public JobOpeningStatus Status { get; set; } = JobOpeningStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual Company Company { get; set; } = null!;
    public virtual CompanyHR CreatedByHR { get; set; } = null!;
    public virtual ICollection<JobTargetCollege> TargetColleges { get; set; } = new List<JobTargetCollege>();
    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    public virtual ICollection<RecruitmentDrive> RecruitmentDrives { get; set; } = new List<RecruitmentDrive>();
    public virtual ICollection<InterviewRound> InterviewRounds { get; set; } = new List<InterviewRound>();
}