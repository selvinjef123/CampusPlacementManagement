using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.API.Enums;

namespace CampusPlacementManagement.API.DTOs.JobApplication;

public class JobApplicationStatusUpdateDto
{
    [Required]
    public ApplicationStatus NewStatus { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }
}