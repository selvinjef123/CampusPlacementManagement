using CampusPlacementManagement.API.DTOs.Student;
using CampusPlacementManagement.API.Enums;
using CampusPlacementManagement.API.Models;
using CampusPlacementManagement.API.Repositories.Interfaces;
using CampusPlacementManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.API.Services;

public class StudentService : IStudentService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPlacementOfficerRepository _officerRepository;
    private readonly IStudentRepository _studentRepository;

    public StudentService(
        UserManager<ApplicationUser> userManager,
        IPlacementOfficerRepository officerRepository,
        IStudentRepository studentRepository)
    {
        _userManager = userManager;
        _officerRepository = officerRepository;
        _studentRepository = studentRepository;
    }

    public async Task<StudentResponseDto> EnrollStudentAsync(string officerUserId, StudentEnrollDto dto)
    {
        var officer = await _officerRepository.GetByUserIdAsync(officerUserId);
        if (officer == null || officer.Status != AccountStatus.Approved)
        {
            throw new InvalidOperationException("Only approved placement officers can enroll students.");
        }

        if (await _studentRepository.RegisterNumberExistsAsync(officer.CollegeId, dto.RegisterNumber))
        {
            throw new InvalidOperationException($"Register number '{dto.RegisterNumber}' already exists in this college.");
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with email '{dto.Email}' already exists.");
        }

        var defaultPassword = $"Std@{dto.RegisterNumber}!";

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            FullName = dto.FullName,
            AccountStatus = AccountStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, defaultPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User creation failed: {errors}");
        }

        await _userManager.AddToRoleAsync(user, "Student");

        var student = new Student
        {
            UserId = user.Id,
            CollegeId = officer.CollegeId,
            PlacementOfficerId = officer.PlacementOfficerId,
            RegisterNumber = dto.RegisterNumber,
            Department = dto.Department,
            Degree = dto.Degree,
            GraduationYear = dto.GraduationYear,
            CGPA = dto.CGPA,
            ActiveBacklogs = dto.ActiveBacklogs,
            Status = AccountStatus.Pending
        };

        await _studentRepository.AddAsync(student);
        return MapToDto(student);
    }

    public async Task<IEnumerable<StudentResponseDto>> GetStudentsByCollegeAsync(int collegeId)
    {
        var students = await _studentRepository.GetByCollegeIdAsync(collegeId);
        return students.Select(MapToDto);
    }

    public async Task<IEnumerable<StudentResponseDto>> GetPendingStudentsByCollegeAsync(int collegeId)
    {
        var students = await _studentRepository.GetPendingByCollegeIdAsync(collegeId);
        return students.Select(MapToDto);
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return students.Select(MapToDto);
    }

    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        return student == null ? null : MapToDto(student);
    }

    public async Task<StudentResponseDto?> GetStudentByUserIdAsync(string userId)
    {
        var student = await _studentRepository.GetByUserIdAsync(userId);
        return student == null ? null : MapToDto(student);
    }

    public async Task<bool> ApproveStudentAsync(int studentId, string officerUserId)
    {
        var officer = await _officerRepository.GetByUserIdAsync(officerUserId);
        var student = await _studentRepository.GetByIdAsync(studentId);

        if (officer == null || student == null || officer.CollegeId != student.CollegeId)
        {
            return false;
        }

        student.Status = AccountStatus.Approved;
        student.ApprovedAt = DateTime.UtcNow;

        var user = await _userManager.FindByIdAsync(student.UserId);
        if (user != null)
        {
            user.AccountStatus = AccountStatus.Approved;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        await _studentRepository.UpdateAsync(student);
        return true;
    }

    public async Task<bool> RejectStudentAsync(int studentId, string officerUserId)
    {
        var officer = await _officerRepository.GetByUserIdAsync(officerUserId);
        var student = await _studentRepository.GetByIdAsync(studentId);

        if (officer == null || student == null || officer.CollegeId != student.CollegeId)
        {
            return false;
        }

        student.Status = AccountStatus.Rejected;

        var user = await _userManager.FindByIdAsync(student.UserId);
        if (user != null)
        {
            user.AccountStatus = AccountStatus.Rejected;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        await _studentRepository.UpdateAsync(student);
        return true;
    }

    public async Task<bool> UpdateStudentAsync(int studentId, StudentUpdateDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null) return false;

        student.Department = dto.Department;
        student.CGPA = dto.CGPA;
        student.ActiveBacklogs = dto.ActiveBacklogs;
        if (!string.IsNullOrEmpty(dto.ResumeUrl))
        {
            student.ResumeUrl = dto.ResumeUrl;
        }

        await _studentRepository.UpdateAsync(student);
        return true;
    }

    private static StudentResponseDto MapToDto(Student s) => new()
    {
        StudentId = s.StudentId,
        UserId = s.UserId,
        FullName = s.User?.FullName ?? string.Empty,
        Email = s.User?.Email ?? string.Empty,
        PhoneNumber = s.User?.PhoneNumber ?? string.Empty,
        CollegeId = s.CollegeId,
        CollegeName = s.College?.CollegeName ?? string.Empty,
        PlacementOfficerId = s.PlacementOfficerId,
        RegisterNumber = s.RegisterNumber,
        Department = s.Department,
        Degree = s.Degree,
        GraduationYear = s.GraduationYear,
        CGPA = s.CGPA,
        ActiveBacklogs = s.ActiveBacklogs,
        ResumeUrl = s.ResumeUrl,
        Status = s.Status,
        ApprovedAt = s.ApprovedAt
    };
}