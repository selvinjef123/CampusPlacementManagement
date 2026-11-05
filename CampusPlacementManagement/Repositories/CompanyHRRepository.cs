using CampusPlacementManagement.API.Data;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Repositories;

public class CompanyHRRepository : ICompanyHRRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyHRRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyHR?> GetByIdAsync(int id)
    {
        return await _context.CompanyHRs
            .Include(chr => chr.User)
            .Include(chr => chr.Company)
            .FirstOrDefaultAsync(chr => chr.CompanyHRId == id);
    }

    public async Task<CompanyHR?> GetByUserIdAsync(string userId)
    {
        return await _context.CompanyHRs
            .Include(chr => chr.User)
            .Include(chr => chr.Company)
            .FirstOrDefaultAsync(chr => chr.UserId == userId);
    }

    public async Task<IEnumerable<CompanyHR>> GetAllAsync()
    {
        return await _context.CompanyHRs
            .Include(chr => chr.User)
            .Include(chr => chr.Company)
            .ToListAsync();
    }

    public async Task<IEnumerable<CompanyHR>> GetPendingHRsAsync()
    {
        return await _context.CompanyHRs
            .Include(chr => chr.User)
            .Include(chr => chr.Company)
            .Where(chr => chr.User.AccountStatus == AccountStatus.Pending)
            .ToListAsync();
    }

    public async Task AddAsync(CompanyHR companyHR)
    {
        await _context.CompanyHRs.AddAsync(companyHR);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CompanyHR companyHR)
    {
        _context.CompanyHRs.Update(companyHR);
        await _context.SaveChangesAsync();
    }
}