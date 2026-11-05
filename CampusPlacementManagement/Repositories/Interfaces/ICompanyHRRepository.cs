using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface ICompanyHRRepository
{
    Task<CompanyHR?> GetByIdAsync(int id);
    Task<CompanyHR?> GetByUserIdAsync(string userId);
    Task<IEnumerable<CompanyHR>> GetAllAsync();
    Task<IEnumerable<CompanyHR>> GetPendingHRsAsync();
    Task AddAsync(CompanyHR companyHR);
    Task UpdateAsync(CompanyHR companyHR);
}