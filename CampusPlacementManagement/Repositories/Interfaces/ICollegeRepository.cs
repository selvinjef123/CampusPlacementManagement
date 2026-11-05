using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface ICollegeRepository
{
    Task<College?> GetByIdAsync(int id);
    Task<IEnumerable<College>> GetAllAsync();
    Task<IEnumerable<College>> GetPendingCollegesAsync();
    Task<bool> CollegeCodeExistsAsync(string code);
    Task AddAsync(College college);
    Task UpdateAsync(College college);
}