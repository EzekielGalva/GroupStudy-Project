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
    public class IndexModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public IndexModel(GroupStudyV3.Models.GroupStudyV2Context context)
        {
            _context = context;
        }

        public IList<StudyGroupMember> StudyGroupMember { get;set; } = default!;

        public async Task OnGetAsync()
        {
            StudyGroupMember = await _context.StudyGroupMembers
                .Include(s => s.Student)
                .Include(s => s.StudyGroup).ToListAsync();
        }
    }
}
