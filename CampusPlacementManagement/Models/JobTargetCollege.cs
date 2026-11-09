using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.Models;

public class JobTargetCollege
{
    [Key]
    public int JobTargetCollegeId { get; set; }
    public int JobId { get; set; }
    public int CollegeId { get; set; }

    // Navigation Properties
    public virtual JobOpening JobOpening { get; set; } = null!;
    public virtual College College { get; set; } = null!;
}