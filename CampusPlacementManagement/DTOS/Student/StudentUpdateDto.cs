using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.Student;

public class StudentUpdateDto
{
    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, Range(0.00, 10.00)]
    public decimal CGPA { get; set; }

    [Required, Range(0, 100)]
    public int ActiveBacklogs { get; set; }

    [Url, StringLength(500)]
    public string? ResumeUrl { get; set; }
}