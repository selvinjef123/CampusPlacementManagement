using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface IPlacementOfficerRepository
{
    Task<PlacementOfficer?> GetByIdAsync(int id);
    Task<PlacementOfficer?> GetByUserIdAsync(string userId);
    Task<IEnumerable<PlacementOfficer>> GetPendingOfficersAsync();
    Task AddAsync(PlacementOfficer officer);
    Task UpdateAsync(PlacementOfficer officer);
}