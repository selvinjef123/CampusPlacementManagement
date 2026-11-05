using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Models;

public class CompanyHR
{
    public int CompanyHRId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string? Designation { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation Properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
    public virtual ApplicationUser? ApprovedByUser { get; set; }
    public virtual ICollection<JobOpening> ManagedJobs { get; set; } = new List<JobOpening>();
}