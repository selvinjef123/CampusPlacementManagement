using CampusPlacementManagement.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<College> Colleges => Set<College>();
    public DbSet<PlacementOfficer> PlacementOfficers => Set<PlacementOfficer>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyHR> CompanyHRs => Set<CompanyHR>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<JobOpening> JobOpenings => Set<JobOpening>();
    public DbSet<JobTargetCollege> JobTargetColleges => Set<JobTargetCollege>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<RecruitmentDrive> RecruitmentDrives => Set<RecruitmentDrive>();
    public DbSet<InterviewRound> InterviewRounds => Set<InterviewRound>();
    public DbSet<InterviewSchedule> InterviewSchedules => Set<InterviewSchedule>();
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories => Set<ApplicationStatusHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // -------------------------------------------------------------
        // ApplicationUser Relationships (1:1 with Profiles)
        // -------------------------------------------------------------
        builder.Entity<PlacementOfficer>()
            .HasOne(po => po.User)
            .WithOne(u => u.PlacementOfficer)
            .HasForeignKey<PlacementOfficer>(po => po.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CompanyHR>()
            .HasOne(chr => chr.User)
            .WithOne(u => u.CompanyHR)
            .HasForeignKey<CompanyHR>(chr => chr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Approver Foreign Keys
        builder.Entity<PlacementOfficer>()
            .HasOne(po => po.ApprovedByUser)
            .WithMany()
            .HasForeignKey(po => po.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CompanyHR>()
            .HasOne(chr => chr.ApprovedByUser)
            .WithMany()
            .HasForeignKey(chr => chr.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // -------------------------------------------------------------
        // College Relationships & Constraints
        // -------------------------------------------------------------
        builder.Entity<College>()
            .HasIndex(c => c.CollegeCode)
            .IsUnique();

        builder.Entity<PlacementOfficer>()
            .HasOne(po => po.College)
            .WithMany(c => c.PlacementOfficers)
            .HasForeignKey(po => po.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Student>()
            .HasOne(s => s.College)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Student>()
            .HasOne(s => s.PlacementOfficer)
            .WithMany(po => po.EnrolledStudents)
            .HasForeignKey(s => s.PlacementOfficerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Composite Unique Index for Register Number per College
        builder.Entity<Student>()
            .HasIndex(s => new { s.CollegeId, s.RegisterNumber })
            .IsUnique();

        // -------------------------------------------------------------
        // Company & Job Opening Relationships
        // -------------------------------------------------------------
        builder.Entity<CompanyHR>()
            .HasOne(chr => chr.Company)
            .WithMany(c => c.CompanyHRs)
            .HasForeignKey(chr => chr.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<JobOpening>()
            .HasOne(j => j.Company)
            .WithMany(c => c.JobOpenings)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<JobOpening>()
            .HasOne(j => j.CreatedByHR)
            .WithMany(chr => chr.ManagedJobs)
            .HasForeignKey(j => j.CreatedByHRId)
            .OnDelete(DeleteBehavior.Restrict);

        // -------------------------------------------------------------
        // Job Target College Junction Table
        // -------------------------------------------------------------
        builder.Entity<JobTargetCollege>()
            .HasOne(jtc => jtc.JobOpening)
            .WithMany(j => j.TargetColleges)
            .HasForeignKey(jtc => jtc.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<JobTargetCollege>()
            .HasOne(jtc => jtc.College)
            .WithMany(c => c.JobTargetColleges)
            .HasForeignKey(jtc => jtc.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);

        // -------------------------------------------------------------
        // Job Application Relationships & Composite Index
        // -------------------------------------------------------------
        builder.Entity<JobApplication>()
            .HasIndex(ja => new { ja.StudentId, ja.JobId })
            .IsUnique();

        builder.Entity<JobApplication>()
            .HasOne(ja => ja.JobOpening)
            .WithMany(j => j.JobApplications)
            .HasForeignKey(ja => ja.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<JobApplication>()
            .HasOne(ja => ja.Student)
            .WithMany(s => s.JobApplications)
            .HasForeignKey(ja => ja.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // -------------------------------------------------------------
        // Recruitment Drive, Interview Round & Schedule
        // -------------------------------------------------------------
        builder.Entity<RecruitmentDrive>()
            .HasOne(rd => rd.JobOpening)
            .WithMany(j => j.RecruitmentDrives)
            .HasForeignKey(rd => rd.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RecruitmentDrive>()
            .HasOne(rd => rd.College)
            .WithMany(c => c.RecruitmentDrives)
            .HasForeignKey(rd => rd.CollegeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<InterviewRound>()
            .HasOne(ir => ir.JobOpening)
            .WithMany(j => j.InterviewRounds)
            .HasForeignKey(ir => ir.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<InterviewSchedule>()
            .HasOne(isch => isch.JobApplication)
            .WithMany(ja => ja.InterviewSchedules)
            .HasForeignKey(isch => isch.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<InterviewSchedule>()
            .HasOne(isch => isch.InterviewRound)
            .WithMany(ir => ir.InterviewSchedules)
            .HasForeignKey(isch => isch.InterviewRoundId)
            .OnDelete(DeleteBehavior.Restrict);

        // -------------------------------------------------------------
        // Application Status History Configuration
        // -------------------------------------------------------------
        builder.Entity<ApplicationStatusHistory>()
            .HasKey(ash => ash.HistoryId);

        builder.Entity<ApplicationStatusHistory>()
            .HasOne(ash => ash.JobApplication)
            .WithMany(ja => ja.StatusHistories)
            .HasForeignKey(ash => ash.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationStatusHistory>()
            .HasOne(ash => ash.UpdatedByUser)
            .WithMany()
            .HasForeignKey(ash => ash.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}