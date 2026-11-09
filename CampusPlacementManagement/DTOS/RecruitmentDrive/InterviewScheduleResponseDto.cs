namespace CampusPlacementManagement.API.DTOs.RecruitmentDrive;

public class InterviewScheduleResponseDto
{
    public int ScheduleId { get; set; }
    public int ApplicationId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RegisterNumber { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public int InterviewRoundId { get; set; }
    public int RoundNumber { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public DateTime ScheduledTime { get; set; }
    public string? LocationOrLink { get; set; }
    public string? Instructions { get; set; }
    public string Status { get; set; } = string.Empty;
}