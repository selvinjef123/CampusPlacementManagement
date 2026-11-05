using CampusPlacementManagement.API.DTOs.PlacementOfficer;
using CampusPlacementManagement.API.Helpers;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacementOfficersController : ControllerBase
{
    private readonly IPlacementOfficerService _officerService;

    public PlacementOfficersController(IPlacementOfficerService officerService)
    {
        _officerService = officerService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] PlacementOfficerRegisterDto dto)
    {
        try
        {
            var result = await _officerService.RegisterAsync(dto);
            if (!result) return BadRequest(new { message = "Registration failed." });

            return Ok(new { message = "Placement Officer registered successfully. Pending SuperAdmin approval." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var adminUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(adminUserId)) return Unauthorized();

        var success = await _officerService.ApproveOfficerAsync(id, adminUserId);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var success = await _officerService.RejectOfficerAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}