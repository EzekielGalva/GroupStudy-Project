using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudentAssignments
{
    public class DeleteModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public DeleteModel(GroupStudyV3.Models.GroupStudyV2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentAssignment StudentAssignment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentassignment = await _context.StudentAssignments.FirstOrDefaultAsync(m => m.StudentAssignmentId == id);

            if (studentassignment is not null)
            {
                StudentAssignment = studentassignment;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentassignment = await _context.StudentAssignments.FindAsync(id);
            if (studentassignment != null)
            {
                StudentAssignment = studentassignment;
                _context.StudentAssignments.Remove(StudentAssignment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
