//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//
//namespace MyApp.Namespace
//{
//    public class GroupByAssignmentModel : PageModel
//    {
//        public void OnGet()
//        {
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.GroupByAssignment
{
    public class GroupByAssignmentModel : PageModel
    {
        private readonly GroupStudyV2Context _ctx;
        public GroupByAssignmentModel(GroupStudyV2Context ctx) => _ctx = ctx;

        // the list of assignments + student data to display
        public List<Assignment> Assignments { get; set; } = new();

        // filter inputs bound from query string
        [BindProperty(SupportsGet = true)]
        public string? Standing { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Gender { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? MinAge { get; set; }

        public async Task OnGetAsync()
        {
            var q = _ctx.Assignments
                        .Include(a => a.StudentAssignments)
                          .ThenInclude(sa => sa.Student)
                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Standing))
                q = q.Where(a => a.StudentAssignments.Any(sa => sa.Student.ClassStanding == Standing));
            if (!string.IsNullOrWhiteSpace(Gender))
                q = q.Where(a => a.StudentAssignments.Any(sa => sa.Student.Gender == Gender));
            if (MinAge.HasValue)
                q = q.Where(a => a.StudentAssignments.Any(sa => sa.Student.Age >= MinAge.Value));

            Assignments = await q.ToListAsync();
        }
    }
}
