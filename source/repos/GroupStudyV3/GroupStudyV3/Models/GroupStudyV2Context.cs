using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GroupStudyV3.Models;

public partial class GroupStudyV2Context : DbContext
{
    public GroupStudyV2Context() { }

    public GroupStudyV2Context(DbContextOptions<GroupStudyV2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<StudentAssignment> StudentAssignments { get; set; }
    public virtual DbSet<StudyGroup> StudyGroups { get; set; }
    public virtual DbSet<StudyGroupMember> StudyGroupMembers { get; set; }
    public virtual DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__Assignme__32499E77EBCEC68D");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B9958371BC1");
            entity.HasIndex(e => e.Email, "UQ__Students__A9D10534543F0CA5").IsUnique();
            entity.Property(e => e.ClassStanding).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<StudentAssignment>(entity =>
        {
            entity.HasKey(e => e.StudentAssignmentId).HasName("PK__StudentA__1DDF88BBD26299BC");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Assignment)
                  .WithMany(p => p.StudentAssignments)
                  .HasForeignKey(d => d.AssignmentId)
                  .OnDelete(DeleteBehavior.Cascade)    // ← changed to Cascade
                  .HasConstraintName("FK_StudentAssignments_Assignments");

            entity.HasOne(d => d.Student)
                  .WithMany(p => p.StudentAssignments)
                  .HasForeignKey(d => d.StudentId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_StudentAssignments_Students");
        });

        modelBuilder.Entity<StudyGroup>(entity =>
        {
            entity.HasKey(e => e.StudyGroupId).HasName("PK__StudyGro__D97A1BE214427EE2");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.GroupName).HasMaxLength(255);

            entity.HasOne(d => d.Assignment)
                  .WithMany(p => p.StudyGroups)
                  .HasForeignKey(d => d.AssignmentId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_StudyGroups_Assignments");

            entity.HasOne(d => d.CreatedByNavigation)
                  .WithMany(p => p.StudyGroups)
                  .HasForeignKey(d => d.CreatedBy)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_StudyGroups_Students");
        });

        modelBuilder.Entity<StudyGroupMember>(entity =>
        {
            entity.HasKey(e => e.StudyGroupMemberId).HasName("PK__StudyGro__F48ED9BAA0A4B084");
            entity.Property(e => e.JoinedOn).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Student)
                  .WithMany(p => p.StudyGroupMembers)
                  .HasForeignKey(d => d.StudentId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_StudyGroupMembers_Students");

            entity.HasOne(d => d.StudyGroup)
                  .WithMany(p => p.StudyGroupMembers)
                  .HasForeignKey(d => d.StudyGroupId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_StudyGroupMembers_Groups");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => c.CourseId);
            entity.Property(c => c.CourseCode)
                  .IsRequired()
                  .HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
