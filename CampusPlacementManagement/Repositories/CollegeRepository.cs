using CampusPlacementManagement.API.Data;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Repositories;

public class CollegeRepository : ICollegeRepository
{
    private readonly ApplicationDbContext _context;

    public CollegeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<College?> GetByIdAsync(int id)
    {
        return await _context.Colleges.FindAsync(id);
    }

    public async Task<IEnumerable<College>> GetAllAsync()
    {
        return await _context.Colleges.ToListAsync();
    }

    public async Task<IEnumerable<College>> GetPendingCollegesAsync()
    {
        return await _context.Colleges.Where(c => c.Status == CollegeStatus.Pending).ToListAsync();
    }

    public async Task<bool> CollegeCodeExistsAsync(string code)
    {
        return await _context.Colleges.AnyAsync(c => c.CollegeCode == code);
    }

    public async Task AddAsync(College college)
    {
        await _context.Colleges.AddAsync(college);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(College college)
    {
        _context.Colleges.Update(college);
        await _context.SaveChangesAsync();
    }
}