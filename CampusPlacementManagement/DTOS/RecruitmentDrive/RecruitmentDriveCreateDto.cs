using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.RecruitmentDrive;

public class RecruitmentDriveCreateDto
{
    [Required]
    public int JobId { get; set; }

    [Required]
    public int CollegeId { get; set; }

    [Required, StringLength(150)]
    public string DriveName { get; set; } = string.Empty;

    [Required]
    public DateTime DriveDate { get; set; }

    [Required, StringLength(200)]
    public string Venue { get; set; } = string.Empty;

    public string? Remarks { get; set; }
}