using CampusPlacementManagement.API.DTOs.College;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollegesController : ControllerBase
{
    private readonly ICollegeService _collegeService;

    public CollegesController(ICollegeService collegeService)
    {
        _collegeService = collegeService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CollegeRegisterDto dto)
    {
        try
        {
            var result = await _collegeService.RegisterCollegeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.CollegeId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var colleges = await _collegeService.GetAllCollegesAsync();
        return Ok(colleges);
    }

    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var colleges = await _collegeService.GetPendingCollegesAsync();
        return Ok(colleges);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
    public async Task<IActionResult> GetById(int id)
    {
        var college = await _collegeService.GetCollegeByIdAsync(id);
        if (college == null) return NotFound();
        return Ok(college);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var success = await _collegeService.UpdateCollegeStatusAsync(id, CollegeStatus.Approved);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var success = await _collegeService.UpdateCollegeStatusAsync(id, CollegeStatus.Rejected);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}/suspend")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Suspend(int id)
    {
        var success = await _collegeService.UpdateCollegeStatusAsync(id, CollegeStatus.Suspended);
        if (!success) return NotFound();
        return NoContent();
    }
}