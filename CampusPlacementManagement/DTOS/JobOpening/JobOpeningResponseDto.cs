using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.DTOs.JobOpening;

public class JobOpeningResponseDto
{
    public int JobId { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public int CreatedByHRId { get; set; }
    public string HRName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Requirements { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal PackageLPA { get; set; }
    public decimal MinCGPA { get; set; }
    public int MaxBacklogs { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public JobOpeningStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TargetCollegeDto> TargetColleges { get; set; } = new();
}

public class TargetCollegeDto
{
    public int CollegeId { get; set; }
    public string CollegeName { get; set; } = string.Empty;
    public string CollegeCode { get; set; } = string.Empty;
}