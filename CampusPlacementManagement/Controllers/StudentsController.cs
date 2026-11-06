using CampusPlacementManagement.API.DTOs.Student;
using CampusPlacementManagement.API.Helpers;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.API.DTOs.PlacementOfficer;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IPlacementOfficerService _officerService;

    public StudentsController(IStudentService studentService, IPlacementOfficerService officerService)
    {
        _studentService = studentService;
        _officerService = officerService;
    }

    [HttpPost("enroll")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> Enroll([FromBody] StudentEnrollDto dto)
    {
        var officerUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(officerUserId)) return Unauthorized();

        try
        {
            var result = await _studentService.EnrollStudentAsync(officerUserId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.StudentId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
    public async Task<IActionResult> GetAll()
    {
        if (User.IsInRole("SuperAdmin"))
        {
            var allStudents = await _studentService.GetAllStudentsAsync();
            return Ok(allStudents);
        }

        var officerUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(officerUserId)) return Unauthorized();

        // Retrieve students belonging only to the officer's college
        var officer = await _officerService.GetByUserIdAsync(officerUserId);
        if (officer == null) return NotFound();

        var collegeStudents = await _studentService.GetStudentsByCollegeAsync(officer.CollegeId);
        return Ok(collegeStudents);
    }

    [HttpGet("pending")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> GetPending()
    {
        var officerUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(officerUserId)) return Unauthorized();

        var officer = await _officerService.GetByUserIdAsync(officerUserId);
        if (officer == null) return NotFound();

        var pendingStudents = await _studentService.GetPendingStudentsByCollegeAsync(officer.CollegeId);
        return Ok(pendingStudents);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var student = await _studentService.GetStudentByUserIdAsync(userId);
        if (student == null) return NotFound();

        return Ok(student);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin,PlacementOfficer,Student")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null) return NotFound();

        return Ok(student);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> Approve(int id)
    {
        var officerUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(officerUserId)) return Unauthorized();

        var success = await _studentService.ApproveStudentAsync(id, officerUserId);
        if (!success) return BadRequest(new { message = "Approval failed. Officer and student must belong to the same college." });

        return NoContent();
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "PlacementOfficer")]
    public async Task<IActionResult> Reject(int id)
    {
        var officerUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(officerUserId)) return Unauthorized();

        var success = await _studentService.RejectStudentAsync(id, officerUserId);
        if (!success) return BadRequest(new { message = "Rejection failed. Officer and student must belong to the same college." });

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,PlacementOfficer,Student")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto)
    {
        var success = await _studentService.UpdateStudentAsync(id, dto);
        if (!success) return NotFound();

        return NoContent();
    }
}