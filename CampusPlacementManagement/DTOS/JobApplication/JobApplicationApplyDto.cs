using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.JobApplication;

public class JobApplicationApplyDto
{
    [Required]
    public int JobId { get; set; }

    [Url, StringLength(500)]
    public string? CoverLetterUrl { get; set; }
}