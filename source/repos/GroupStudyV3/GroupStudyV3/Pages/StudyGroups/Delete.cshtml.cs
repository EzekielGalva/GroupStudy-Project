using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudyGroups
{
    public class DeleteModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public DeleteModel(GroupStudyV3.Models.GroupStudyV2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public StudyGroup StudyGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studygroup = await _context.StudyGroups.FirstOrDefaultAsync(m => m.StudyGroupId == id);

            if (studygroup is not null)
            {
                StudyGroup = studygroup;

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

            var studygroup = await _context.StudyGroups.FindAsync(id);
            if (studygroup != null)
            {
                StudyGroup = studygroup;
                _context.StudyGroups.Remove(StudyGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
