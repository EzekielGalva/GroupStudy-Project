using System;
using System.Collections.Generic;

namespace GroupStudyV3.Models;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<StudentAssignment> StudentAssignments { get; set; } = new List<StudentAssignment>();

    public virtual ICollection<StudyGroup> StudyGroups { get; set; } = new List<StudyGroup>();

    // foreign-key field:
    public int CourseId { get; set; }

    // navigation property:
    public Course? Course { get; set; }

}
