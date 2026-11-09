using CampusPlacementManagement.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.Models;

public class PlacementOfficer
{
    [Key]
    public int PlacementOfficerId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CollegeId { get; set; }
    public string Designation { get; set; } = string.Empty;
    public AccountStatus Status { get; set; } = AccountStatus.Pending;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation Properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual College College { get; set; } = null!;
    public virtual ApplicationUser? ApprovedByUser { get; set; }
    public virtual ICollection<Student> EnrolledStudents { get; set; } = new List<Student>();
}