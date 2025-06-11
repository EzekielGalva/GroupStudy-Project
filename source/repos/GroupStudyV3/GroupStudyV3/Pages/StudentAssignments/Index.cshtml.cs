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
    public class IndexModel : PageModel
    {
        private readonly GroupStudyV3.Models.GroupStudyV2Context _context;

        public IndexModel(GroupStudyV3.Models.GroupStudyV2Context context)
        {
            _context = context;
        }

        public List<Student> StudentList { get; private set; } = new();

        public Student? CurrentStudent { get; private set; }



        public IList<StudentAssignment> StudentAssignment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            StudentAssignment = await _context.StudentAssignments
                .Include(s => s.Assignment)
                .Include(s => s.Student).ToListAsync();
        }
    }
}
