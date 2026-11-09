using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Models;

public class RecruitmentDrive
{
    [Key]
    public int DriveId { get; set; }
    public int JobId { get; set; }
    public int CollegeId { get; set; }
    public DateTime DriveDate { get; set; }
    public string? Venue { get; set; }
    public string? Description { get; set; }
    public RecruitmentDriveStatus Status { get; set; } = RecruitmentDriveStatus.Scheduled;

    // Navigation Properties
    public virtual JobOpening JobOpening { get; set; } = null!;
    public virtual College College { get; set; } = null!;
}