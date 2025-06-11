using System.Collections.Generic;

namespace GroupStudyV3.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = null!;
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
