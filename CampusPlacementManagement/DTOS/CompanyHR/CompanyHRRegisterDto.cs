using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.CompanyHR;

public class CompanyHRRegisterDto
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int CompanyId { get; set; }

    [StringLength(100)]
    public string? Designation { get; set; }
}