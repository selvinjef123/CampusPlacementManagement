using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.DTOs.PlacementOfficer;

public class PlacementOfficerResponseDto
{
    public int PlacementOfficerId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CollegeId { get; set; }
    public string CollegeName { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public AccountStatus Status { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
}