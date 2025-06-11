using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudentAssignments
{
    public class CreateModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public CreateModel(GroupStudyV3.Models.GroupStudyV2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "AssignmentId", "AssignmentId");
        ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return Page();
        }

        [BindProperty]
        public StudentAssignment StudentAssignment { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.StudentAssignments.Add(StudentAssignment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
