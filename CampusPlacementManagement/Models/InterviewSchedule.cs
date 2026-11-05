using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Models;

public class InterviewSchedule
{
    public int InterviewScheduleId { get; set; }
    public int ApplicationId { get; set; }
    public int InterviewRoundId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string? MeetingLink { get; set; }
    public string? Venue { get; set; }
    public InterviewStatus Status { get; set; } = InterviewStatus.Scheduled;
    public string? Feedback { get; set; }

    // Navigation Properties
    public virtual JobApplication JobApplication { get; set; } = null!;
    public virtual InterviewRound InterviewRound { get; set; } = null!;
}