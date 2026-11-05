using CampusPlacementManagement.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.API.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public AccountStatus AccountStatus { get; set; } = AccountStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual PlacementOfficer? PlacementOfficer { get; set; }
    public virtual CompanyHR? CompanyHR { get; set; }
    public virtual Student? Student { get; set; }
}