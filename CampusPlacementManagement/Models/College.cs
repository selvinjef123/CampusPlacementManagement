using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.Models;

public class College
{
    public int CollegeId { get; set; }
    public string CollegeName { get; set; } = string.Empty;
    public string CollegeCode { get; set; } = string.Empty;
    public string? UniversityName { get; set; }
    public string OfficialEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Website { get; set; }
    public CollegeStatus Status { get; set; } = CollegeStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual ICollection<PlacementOfficer> PlacementOfficers { get; set; } = new List<PlacementOfficer>();
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    public virtual ICollection<JobTargetCollege> JobTargetColleges { get; set; } = new List<JobTargetCollege>();
    public virtual ICollection<RecruitmentDrive> RecruitmentDrives { get; set; } = new List<RecruitmentDrive>();
}