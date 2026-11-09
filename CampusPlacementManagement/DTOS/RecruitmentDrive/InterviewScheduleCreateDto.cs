using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.RecruitmentDrive;

public class InterviewScheduleCreateDto
{
    [Required]
    public int ApplicationId { get; set; }

    [Required]
    public int InterviewRoundId { get; set; }

    [Required]
    public DateTime ScheduledTime { get; set; }

    [StringLength(200)]
    public string? LocationOrLink { get; set; }

    public string? Instructions { get; set; }
}