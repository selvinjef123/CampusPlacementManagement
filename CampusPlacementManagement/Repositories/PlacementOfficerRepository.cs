using CampusPlacementManagement.API.Data;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Repositories;

public class PlacementOfficerRepository : IPlacementOfficerRepository
{
    private readonly ApplicationDbContext _context;

    public PlacementOfficerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PlacementOfficer?> GetByIdAsync(int id)
    {
        return await _context.PlacementOfficers
            .Include(po => po.User)
            .Include(po => po.College)
            .FirstOrDefaultAsync(po => po.PlacementOfficerId == id);
    }

    public async Task<PlacementOfficer?> GetByUserIdAsync(string userId)
    {
        return await _context.PlacementOfficers
            .Include(po => po.User)
            .Include(po => po.College)
            .FirstOrDefaultAsync(po => po.UserId == userId);
    }

    public async Task<IEnumerable<PlacementOfficer>> GetPendingOfficersAsync()
    {
        return await _context.PlacementOfficers
            .Include(po => po.User)
            .Include(po => po.College)
            .Where(po => po.Status == AccountStatus.Pending)
            .ToListAsync();
    }

    public async Task AddAsync(PlacementOfficer officer)
    {
        await _context.PlacementOfficers.AddAsync(officer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PlacementOfficer officer)
    {
        _context.PlacementOfficers.Update(officer);
        await _context.SaveChangesAsync();
    }
}