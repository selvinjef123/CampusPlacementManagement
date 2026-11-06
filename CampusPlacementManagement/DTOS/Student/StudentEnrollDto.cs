using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.Student;

public class StudentEnrollDto
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string RegisterNumber { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Degree { get; set; } = string.Empty;

    [Required, Range(2000, 2100)]
    public int GraduationYear { get; set; }

    [Required, Range(0.00, 10.00)]
    public decimal CGPA { get; set; }

    [Required, Range(0, 100)]
    public int ActiveBacklogs { get; set; }
}