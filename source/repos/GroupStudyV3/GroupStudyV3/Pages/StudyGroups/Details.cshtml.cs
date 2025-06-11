using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudyGroups
{
    public class DetailsModel : PageModel
    {
        private readonly GroupStudyV2Context _context;
        public DetailsModel(GroupStudyV2Context context) => _context = context;

        public StudyGroup? Group { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Group = await _context.StudyGroups
                .Include(g => g.Assignment)               // the assignment the group is for
                .Include(g => g.CreatedByNavigation)      // the creator
                .Include(g => g.StudyGroupMembers)        // the membership
                   .ThenInclude(m => m.Student)           // each member
                      .ThenInclude(s => s.StudentAssignments)  // each member’s assignments
                         .ThenInclude(sa => sa.Assignment)     // assignment details
                .FirstOrDefaultAsync(g => g.StudyGroupId == id);

            if (Group == null) return NotFound();

            return Page();
        }
    }
}
