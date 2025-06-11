using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.Assignments
{
    public class DetailsModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public DetailsModel(GroupStudyV3.Models.GroupStudyV2Context context)
        {
            _context = context;
        }

        public Assignment Assignment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FirstOrDefaultAsync(m => m.AssignmentId == id);

            if (assignment is not null)
            {
                Assignment = assignment;

                return Page();
            }

            return NotFound();
        }
    }
}
