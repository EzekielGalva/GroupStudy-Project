using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.StudyGroups
{
    public class IndexModel : PageModel
    {
        private readonly GroupStudyV2Context _context;
        public IndexModel(GroupStudyV2Context context) => _context = context;

        public IList<StudyGroup> MyGroups { get; private set; } = default!;

        public async Task OnGetAsync()
        {
            var currentId = HttpContext.Session.GetInt32("CurrentStudentId");
            if (currentId == null)
            {
                MyGroups = new List<StudyGroup>();
                return;
            }

            MyGroups = await _context.StudyGroups
                .Include(g => g.Assignment)
                .Include(g => g.CreatedByNavigation)
                .Include(g => g.StudyGroupMembers)
                   .ThenInclude(m => m.Student)
                .Where(g => g.StudyGroupMembers.Any(m => m.StudentId == currentId.Value))
                .ToListAsync();
        }
    }
}
