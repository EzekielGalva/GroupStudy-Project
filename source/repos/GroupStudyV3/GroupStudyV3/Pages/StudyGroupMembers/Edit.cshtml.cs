using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudyGroupMembers
{
    public class EditModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public EditModel(GroupStudyV3.Models.GroupStudyV2Context context)
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

            var studygroupmember =  await _context.StudyGroupMembers.FirstOrDefaultAsync(m => m.StudyGroupMemberId == id);
            if (studygroupmember == null)
            {
                return NotFound();
            }
            StudyGroupMember = studygroupmember;
           ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
           ViewData["StudyGroupId"] = new SelectList(_context.StudyGroups, "StudyGroupId", "StudyGroupId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(StudyGroupMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupMemberExists(StudyGroupMember.StudyGroupMemberId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StudyGroupMemberExists(int id)
        {
            return _context.StudyGroupMembers.Any(e => e.StudyGroupMemberId == id);
        }
    }
}
