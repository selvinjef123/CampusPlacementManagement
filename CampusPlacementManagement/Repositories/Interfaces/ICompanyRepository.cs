using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(int id);
    Task<IEnumerable<Company>> GetAllAsync();
    Task<IEnumerable<Company>> GetPendingCompaniesAsync();
    Task<bool> CompanyEmailExistsAsync(string email);
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
}