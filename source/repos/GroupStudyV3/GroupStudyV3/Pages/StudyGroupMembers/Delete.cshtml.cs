using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudyGroupMembers
{
    public class DeleteModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public DeleteModel(GroupStudyV3.Models.GroupStudyV2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public StudyGroupMember StudyGroupMember { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studygroupmember = await _context.StudyGroupMembers.FirstOrDefaultAsync(m => m.StudyGroupMemberId == id);

            if (studygroupmember is not null)
            {
                StudyGroupMember = studygroupmember;

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

            var studygroupmember = await _context.StudyGroupMembers.FindAsync(id);
            if (studygroupmember != null)
            {
                StudyGroupMember = studygroupmember;
                _context.StudyGroupMembers.Remove(StudyGroupMember);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
