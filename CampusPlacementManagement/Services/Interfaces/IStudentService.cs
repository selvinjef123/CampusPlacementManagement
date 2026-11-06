using CampusPlacementManagement.API.DTOs.Student;

namespace CampusPlacementManagement.API.Services.Interfaces;

public interface IStudentService
{
    Task<StudentResponseDto> EnrollStudentAsync(string officerUserId, StudentEnrollDto dto);
    Task<IEnumerable<StudentResponseDto>> GetStudentsByCollegeAsync(int collegeId);
    Task<IEnumerable<StudentResponseDto>> GetPendingStudentsByCollegeAsync(int collegeId);
    Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
    Task<StudentResponseDto?> GetStudentByIdAsync(int id);
    Task<StudentResponseDto?> GetStudentByUserIdAsync(string userId);
    Task<bool> ApproveStudentAsync(int studentId, string officerUserId);
    Task<bool> RejectStudentAsync(int studentId, string officerUserId);
    Task<bool> UpdateStudentAsync(int studentId, StudentUpdateDto dto);
}