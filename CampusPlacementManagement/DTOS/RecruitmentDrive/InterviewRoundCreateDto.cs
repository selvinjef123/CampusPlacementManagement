using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.RecruitmentDrive;

public class InterviewRoundCreateDto
{
    [Required]
    public int JobId { get; set; }

    [Required, Range(1, 20)]
    public int RoundNumber { get; set; }

    [Required, StringLength(100)]
    public string RoundName { get; set; } = string.Empty;

    public string? Description { get; set; }
}