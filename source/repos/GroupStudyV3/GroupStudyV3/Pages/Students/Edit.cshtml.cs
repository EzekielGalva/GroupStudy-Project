using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly GroupStudyV2Context _context;
        public EditModel(GroupStudyV2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        // Dropdown for ClassStanding
        public SelectList StandingList { get; set; } = default!;

        private void PopulateStandingList()
        {
            var standings = new[] { "Freshman", "Sophomore", "Junior", "Senior" };
            StandingList = new SelectList(standings);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            
            var me = HttpContext.Session.GetInt32("CurrentStudentId");
            if (me != id)
                return Forbid();

            Student = await _context.Students.FindAsync(id.Value);
            if (Student == null)
                return NotFound();

            PopulateStandingList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            var me = HttpContext.Session.GetInt32("CurrentStudentId");
            if (me != Student.StudentId)
                return Forbid();

            if (!ModelState.IsValid)
            {
                PopulateStandingList();
                return Page();
            }

            _context.Attach(Student).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
