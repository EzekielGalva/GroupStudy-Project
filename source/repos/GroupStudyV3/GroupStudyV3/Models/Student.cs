using System;
using System.Collections.Generic;

namespace GroupStudyV3.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? ClassStanding { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public virtual ICollection<StudentAssignment> StudentAssignments { get; set; } = new List<StudentAssignment>();

    public virtual ICollection<StudyGroupMember> StudyGroupMembers { get; set; } = new List<StudyGroupMember>();

    public virtual ICollection<StudyGroup> StudyGroups { get; set; } = new List<StudyGroup>();
}
