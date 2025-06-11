using System;
using System.Collections.Generic;

namespace GroupStudyV3.Models;

public partial class StudyGroup
{
    public int StudyGroupId { get; set; }

    public int AssignmentId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? GroupName { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual Student CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<StudyGroupMember> StudyGroupMembers { get; set; } = new List<StudyGroupMember>();
}
