using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudyGroups
{
    public class EditModel : PageModel
    {
        private readonly GroupStudyV2Context _ctx;
        public EditModel(GroupStudyV2Context ctx) => _ctx = ctx;

        [BindProperty] public int StudyGroupId { get; set; }
        [BindProperty] public string GroupName { get; set; } = default!;
        [BindProperty] public List<int> SelectedStudentIds { get; set; } = new();

        public List<Student> AllStudents { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var group = await _ctx.StudyGroups.FindAsync(id);
            if (group == null) return NotFound();

            StudyGroupId = group.StudyGroupId;
            GroupName = group.GroupName;

            AllStudents = await _ctx.Students
                                    .OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                                    .ToListAsync();

            SelectedStudentIds = await _ctx.StudyGroupMembers
                                          .Where(m => m.StudyGroupId == id)
                                          .Select(m => m.StudentId)
                                          .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // load existing membership
            var existing = await _ctx.StudyGroupMembers
                                     .Where(m => m.StudyGroupId == StudyGroupId)
                                     .ToListAsync();

            // remove unselected
            var toRemove = existing.Where(m => !SelectedStudentIds.Contains(m.StudentId)).ToList();
            _ctx.StudyGroupMembers.RemoveRange(toRemove);

            // add newly checked
            var already = existing.Select(m => m.StudentId).ToHashSet();
            var toAdd = SelectedStudentIds
                        .Where(id => !already.Contains(id))
                        .Select(id => new StudyGroupMember
                        {
                            StudyGroupId = StudyGroupId,
                            StudentId = id
                        });
            _ctx.StudyGroupMembers.AddRange(toAdd);

            await _ctx.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
