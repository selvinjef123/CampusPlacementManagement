using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.API.DTOs.PlacementOfficer;

public class PlacementOfficerRegisterDto
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
    public int CollegeId { get; set; }

    [Required, StringLength(100)]
    public string Designation { get; set; } = string.Empty;
}