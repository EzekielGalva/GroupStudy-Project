using System;
using System.Collections.Generic;

namespace GroupStudyV3.Models;

public partial class StudyGroupMember
{
    public int StudyGroupMemberId { get; set; }

    public int StudyGroupId { get; set; }

    public int StudentId { get; set; }

    public DateTime JoinedOn { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual StudyGroup StudyGroup { get; set; } = null!;
}
