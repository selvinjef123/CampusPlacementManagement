using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.DTOs.JobApplication;

public class JobApplicationResponseDto
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string RegisterNumber { get; set; } = string.Empty;
    public string CollegeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal CGPA { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime AppliedAt { get; set; }
    public string? CoverLetterUrl { get; set; }
    public List<ApplicationStatusHistoryDto> StatusHistories { get; set; } = new();
}

public class ApplicationStatusHistoryDto
{
    public int HistoryId { get; set; }
    public ApplicationStatus? PreviousStatus { get; set; }
    public ApplicationStatus NewStatus { get; set; }
    public string UpdatedByUserId { get; set; } = string.Empty;
    public string UpdatedByUserName { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTime UpdatedAt { get; set; }
}