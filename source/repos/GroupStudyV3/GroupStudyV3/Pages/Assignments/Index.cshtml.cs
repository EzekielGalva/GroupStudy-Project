using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.Assignments
{
    public class IndexModel : PageModel
    {
        private readonly GroupStudyV2Context _context;
        public IndexModel(GroupStudyV2Context context)
        {
            _context = context;
        }

        
        public IList<Assignment> Assignment { get; set; } = default!;

        public async Task OnGetAsync()
        {
            
            var me = HttpContext.Session.GetInt32("CurrentStudentId");
            if (me.HasValue)
            {
                Assignment = await _context.StudentAssignments
                                           .Where(sa => sa.StudentId == me.Value)
                                           .Select(sa => sa.Assignment!)
                                           .ToListAsync();
            }
            else
            {
                
                Assignment = new List<Assignment>();
            }
        }
    }
}
// working