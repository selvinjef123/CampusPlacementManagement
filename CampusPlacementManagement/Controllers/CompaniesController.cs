using CampusPlacementManagement.API.DTOs.Company;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CompanyRegisterDto dto)
    {
        try
        {
            var result = await _companyService.RegisterCompanyAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.CompanyId }, result);
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
        var companies = await _companyService.GetAllCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var companies = await _companyService.GetPendingCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin,CompanyHR")]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _companyService.GetCompanyByIdAsync(id);
        if (company == null) return NotFound();
        return Ok(company);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Approve(int id)
    {
        var success = await _companyService.UpdateCompanyStatusAsync(id, CompanyStatus.Approved);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Reject(int id)
    {
        var success = await _companyService.UpdateCompanyStatusAsync(id, CompanyStatus.Rejected);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPut("{id}/suspend")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Suspend(int id)
    {
        var success = await _companyService.UpdateCompanyStatusAsync(id, CompanyStatus.Suspended);
        if (!success) return NotFound();
        return NoContent();
    }
}