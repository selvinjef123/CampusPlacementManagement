using CampusPlacementManagement.API.DTOs.JobOpening;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Helpers;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobOpeningsController : ControllerBase
{
    private readonly IJobOpeningService _jobService;
    private readonly IPlacementOfficerService _officerService;
    private readonly IStudentService _studentService;
    private readonly ICompanyHRService _hrService;

    public JobOpeningsController(
        IJobOpeningService jobService,
        IPlacementOfficerService officerService,
        IStudentService studentService,
        ICompanyHRService hrService)
    {
        _jobService = jobService;
        _officerService = officerService;
        _studentService = studentService;
        _hrService = hrService;
    }

    [HttpPost]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> CreateJob([FromBody] JobOpeningCreateDto dto)
    {
        var hrUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(hrUserId)) return Unauthorized();

        try
        {
            var result = await _jobService.CreateJobAsync(hrUserId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.JobId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var userId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        if (User.IsInRole("SuperAdmin"))
        {
            var allJobs = await _jobService.GetAllJobsAsync();
            return Ok(allJobs);
        }

        if (User.IsInRole("CompanyHR"))
        {
            var hr = await _hrService.GetByUserIdAsync(userId);
            if (hr == null) return NotFound();

            var companyJobs = await _jobService.GetJobsForCompanyAsync(hr.CompanyId);
            return Ok(companyJobs);
        }

        if (User.IsInRole("PlacementOfficer"))
        {
            var officer = await _officerService.GetByUserIdAsync(userId);
            if (officer == null) return NotFound();

            var collegeJobs = await _jobService.GetTargetedJobsForCollegeAsync(officer.CollegeId);
            return Ok(collegeJobs);
        }

        if (User.IsInRole("Student"))
        {
            var student = await _studentService.GetStudentByUserIdAsync(userId);
            if (student == null) return NotFound();

            var collegeJobs = await _jobService.GetTargetedJobsForCollegeAsync(student.CollegeId);
            return Ok(collegeJobs);
        }

        return Forbid();
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _jobService.GetJobByIdAsync(id);
        if (job == null) return NotFound();

        return Ok(job);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "SuperAdmin,CompanyHR")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] JobOpeningStatus status)
    {
        var success = await _jobService.UpdateJobStatusAsync(id, status);
        if (!success) return NotFound();

        return NoContent();
    }
}