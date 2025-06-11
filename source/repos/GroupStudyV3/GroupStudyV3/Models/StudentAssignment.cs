using System;
using System.Collections.Generic;

namespace GroupStudyV3.Models;

public partial class StudentAssignment
{
    public int StudentAssignmentId { get; set; }

    public int StudentId { get; set; }

    public int AssignmentId { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
