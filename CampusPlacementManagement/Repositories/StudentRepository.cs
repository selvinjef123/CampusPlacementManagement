using CampusPlacementManagement.API.Data;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.College)
            .Include(s => s.PlacementOfficer)
            .FirstOrDefaultAsync(s => s.StudentId == id);
    }

    public async Task<Student?> GetByUserIdAsync(string userId)
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.College)
            .Include(s => s.PlacementOfficer)
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task<IEnumerable<Student>> GetByCollegeIdAsync(int collegeId)
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.College)
            .Where(s => s.CollegeId == collegeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetPendingByCollegeIdAsync(int collegeId)
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.College)
            .Where(s => s.CollegeId == collegeId && s.Status == AccountStatus.Pending)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students
            .Include(s => s.User)
            .Include(s => s.College)
            .ToListAsync();
    }

    public async Task<bool> RegisterNumberExistsAsync(int collegeId, string registerNumber)
    {
        return await _context.Students
            .AnyAsync(s => s.CollegeId == collegeId && s.RegisterNumber.ToLower() == registerNumber.ToLower());
    }

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }
}