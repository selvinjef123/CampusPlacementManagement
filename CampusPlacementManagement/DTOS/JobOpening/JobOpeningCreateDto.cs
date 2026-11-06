using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.JobOpening;

public class JobOpeningCreateDto
{
    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Requirements { get; set; }

    [Required, StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [Required, Range(0, 100000000)]
    public decimal PackageLPA { get; set; }

    [Required, Range(0.00, 10.00)]
    public decimal MinCGPA { get; set; }

    [Required, Range(0, 100)]
    public int MaxBacklogs { get; set; }

    [Required]
    public DateTime ApplicationDeadline { get; set; }

    [Required, MinLength(1, ErrorMessage = "At least one target college must be selected.")]
    public List<int> TargetCollegeIds { get; set; } = new();
}