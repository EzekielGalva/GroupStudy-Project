using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly GroupStudyV2Context _context;

        public CreateModel(GroupStudyV2Context context)
        {
            _context = context;
        }

        // The dropdown values
        public SelectList StandingList { get; set; } = default!;

        private void PopulateStandingList()
        {
            var standings = new[] { "Freshman", "Sophomore", "Junior", "Senior" };
            StandingList = new SelectList(standings);
        }

        public IActionResult OnGet()
        {
            PopulateStandingList();
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateStandingList();
                return Page();
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

           
            HttpContext.Session.SetInt32("CurrentStudentId", Student.StudentId);

            return RedirectToPage("/Index");
        }
    }
}
