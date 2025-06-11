using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.Assignments
{
    public class CreateModel : PageModel
    {
        private readonly GroupStudyV2Context _context;
        public CreateModel(GroupStudyV2Context context)
        {
            _context = context;
        }

        // Dropdowns for Course and Type (you already had these)
        public SelectList Courses { get; set; } = default!;
        public SelectList AssignmentTypes { get; set; } = default!;

        [BindProperty]
        public Assignment Assignment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateListsAsync();
                return Page();
            }

            
            _context.Assignments.Add(Assignment);
            await _context.SaveChangesAsync();

            
            var sid = HttpContext.Session.GetInt32("CurrentStudentId");
            if (sid.HasValue)
            {
                _context.StudentAssignments.Add(new StudentAssignment
                {
                    AssignmentId = Assignment.AssignmentId,
                    StudentId = sid.Value
                });
                await _context.SaveChangesAsync();
            }

            
            return RedirectToPage("./Index");
        }

        private async Task PopulateListsAsync()
        {
            var courses = await _context.Courses
                                        .OrderBy(c => c.CourseCode)
                                        .ToListAsync();
            Courses = new SelectList(courses, nameof(Course.CourseId), nameof(Course.CourseCode));

            var types = new[] { "Homework", "Quiz", "Test", "Project" };
            AssignmentTypes = new SelectList(types);
        }
    }
}
