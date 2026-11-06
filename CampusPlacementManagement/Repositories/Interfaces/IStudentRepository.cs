using CampusPlacementManagement.API.Models;

namespace CampusPlacementManagement.API.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(int id);
    Task<Student?> GetByUserIdAsync(string userId);
    Task<IEnumerable<Student>> GetByCollegeIdAsync(int collegeId);
    Task<IEnumerable<Student>> GetPendingByCollegeIdAsync(int collegeId);
    Task<IEnumerable<Student>> GetAllAsync();
    Task<bool> RegisterNumberExistsAsync(int collegeId, string registerNumber);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
}