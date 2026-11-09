using CampusPlacementManagement.API.DTOs.JobApplication;
using CampusPlacementManagement.API.Helpers;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _applicationService;
    private readonly IPlacementOfficerService _officerService;

    public JobApplicationsController(
        IJobApplicationService applicationService,
        IPlacementOfficerService officerService)
    {
        _applicationService = applicationService;
        _officerService = officerService;
    }

    [HttpPost("apply")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Apply([FromBody] JobApplicationApplyDto dto)
    {
        var studentUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(studentUserId)) return Unauthorized();

        try
        {
            var result = await _applicationService.ApplyForJobAsync(studentUserId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.ApplicationId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my-applications")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyApplications()
    {
        var studentUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(studentUserId)) return Unauthorized();

        var apps = await _applicationService.GetStudentApplicationsAsync(studentUserId);
        return Ok(apps);
    }

    [HttpGet("job/{jobId}")]
    [Authorize(Roles = "SuperAdmin,CompanyHR")]
    public async Task<IActionResult> GetByJob(int jobId)
    {
        var apps = await _applicationService.GetApplicationsForJobAsync(jobId);
        return Ok(apps);
    }

    [HttpGet("college")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> GetCollegeApplications()
    {
        var officerUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(officerUserId)) return Unauthorized();

        var officer = await _officerService.GetByUserIdAsync(officerUserId);
        if (officer == null) return NotFound();

        var apps = await _applicationService.GetApplicationsForCollegeAsync(officer.CollegeId);
        return Ok(apps);
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var apps = await _applicationService.GetAllApplicationsAsync();
        return Ok(apps);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var app = await _applicationService.GetApplicationByIdAsync(id);
        if (app == null) return NotFound();

        return Ok(app);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "SuperAdmin,CompanyHR")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] JobApplicationStatusUpdateDto dto)
    {
        var userId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var success = await _applicationService.UpdateApplicationStatusAsync(id, userId, dto);
        if (!success) return NotFound();

        return NoContent();
    }
}