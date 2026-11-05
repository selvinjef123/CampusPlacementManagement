using CampusPlacementManagement.API.DTOs.CompanyHR;
using CampusPlacementManagement.API.Helpers;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyHRsController : ControllerBase
{
    private readonly ICompanyHRService _hrService;

    public CompanyHRsController(ICompanyHRService hrService)
    {
        _hrService = hrService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CompanyHRRegisterDto dto)
    {
        try
        {
            var result = await _hrService.RegisterAsync(dto);
            if (!result) return BadRequest(new { message = "Registration failed." });

            return Ok(new { message = "Company HR registered successfully. Pending SuperAdmin approval." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var pendingHRs = await _hrService.GetPendingHRsAsync();
        return Ok(pendingHRs);
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var hrs = await _hrService.GetAllHRsAsync();
        return Ok(hrs);
    }

    [HttpGet("me")]
    [Authorize(Roles = "CompanyHR")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var hr = await _hrService.GetByUserIdAsync(userId);
        if (hr == null) return NotFound();

        return Ok(hr);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var adminUserId = CurrentUserHelper.GetUserId(User);
        if (string.IsNullOrEmpty(adminUserId)) return Unauthorized();

        var success = await _hrService.ApproveHRAsync(id, adminUserId);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var success = await _hrService.RejectHRAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}