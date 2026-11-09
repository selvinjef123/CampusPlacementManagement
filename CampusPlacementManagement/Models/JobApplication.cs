using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Models;

public class JobApplication
{
    [Key]
    public int ApplicationId { get; set; }

    public int JobId { get; set; }
    public int StudentId { get; set; }

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    [StringLength(500)]
    public string? CoverLetterUrl { get; set; }

    [StringLength(500)]
    public string? ResumeUrl { get; set; }

    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual JobOpening JobOpening { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
    public virtual ICollection<InterviewSchedule> InterviewSchedules { get; set; } = new List<InterviewSchedule>();
    public virtual ICollection<ApplicationStatusHistory> StatusHistories { get; set; } = new List<ApplicationStatusHistory>();
}