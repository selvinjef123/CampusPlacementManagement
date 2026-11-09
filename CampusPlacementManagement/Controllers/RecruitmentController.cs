using CampusPlacementManagement.API.DTOs.RecruitmentDrive;
using CampusPlacementManagement.API.Helpers;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecruitmentController : ControllerBase
{
    private readonly IRecruitmentService _recruitmentService;

    public RecruitmentController(IRecruitmentService recruitmentService)
    {
        _recruitmentService = recruitmentService;
    }

    [HttpPost("drives")]
    [Authorize(Roles = "SuperAdmin,CompanyHR,PlacementOfficer")]
    public async Task<IActionResult> CreateDrive([FromBody] RecruitmentDriveCreateDto dto)
    {
        var drive = await _recruitmentService.CreateDriveAsync(dto);
        return Ok(drive);
    }

    [HttpGet("drives/college/{collegeId}")]
    [Authorize(Roles = "SuperAdmin,PlacementOfficer,Student")]
    public async Task<IActionResult> GetDrivesByCollege(int collegeId)
    {
        var drives = await _recruitmentService.GetDrivesByCollegeAsync(collegeId);
        return Ok(drives);
    }

    [HttpPost("rounds")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> CreateRound([FromBody] InterviewRoundCreateDto dto)
    {
        var round = await _recruitmentService.CreateRoundAsync(dto);
        return Ok(round);
    }

    [HttpGet("rounds/job/{jobId}")]
    [Authorize]
    public async Task<IActionResult> GetRoundsByJob(int jobId)
    {
        var rounds = await _recruitmentService.GetRoundsByJobAsync(jobId);
        return Ok(rounds);
    }

    [HttpPost("schedules")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> ScheduleInterview([FromBody] InterviewScheduleCreateDto dto)
    {
        try
        {
            var result = await _recruitmentService.ScheduleInterviewAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("schedules/my-interviews")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyInterviews()
    {
        var studentUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(studentUserId)) return Unauthorized();

        var schedules = await _recruitmentService.GetStudentSchedulesAsync(studentUserId);
        return Ok(schedules);
    }
}