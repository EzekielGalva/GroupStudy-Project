//working
//using System.Linq;
//using System.Threading.Tasks;
//using GroupStudyV3.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//
//namespace GroupStudyV3.Pages
//{
//    public class IndexModel : PageModel
//    {
//        private readonly GroupStudyV2Context _context;
//        private readonly ILogger<IndexModel> _logger;
//
//        public IndexModel(GroupStudyV2Context context, ILogger<IndexModel> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }
//
//       
//        [BindProperty]
//        public int? CurrentStudentId { get; set; }
//
//        public Student? CurrentStudent { get; private set; }
//
//        public SelectList StudentList { get; private set; } = default!;
//
//        public async Task OnGetAsync()
//        {
//            var students = await _context.Students
//                                         .OrderBy(s => s.Email)
//                                         .ToListAsync();
//
//            var savedId = HttpContext.Session.GetInt32("CurrentStudentId");
//
//            StudentList = new SelectList(
//                items: students,
//                dataValueField: nameof(Student.StudentId),
//                dataTextField: nameof(Student.Email),
//                selectedValue: savedId
//            );
//
//            if (savedId.HasValue)
//            {
//                CurrentStudentId = savedId;
//                CurrentStudent = await _context.Students.FindAsync(savedId.Value);
//            }
//        }
//
//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (CurrentStudentId.HasValue)
//            {
//                HttpContext.Session.SetInt32("CurrentStudentId", CurrentStudentId.Value);
//            }
//            return RedirectToPage();
//        }
//    }
//}
//


using System.Linq;
using System.Threading.Tasks;
using GroupStudyV3.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GroupStudyV3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly GroupStudyV2Context _context;
        public IndexModel(GroupStudyV2Context context) => _context = context;

        public int? CurrentStudentId { get; private set; }
        public Student? CurrentStudent { get; private set; }
        public SelectList StudentList { get; private set; } = default!;

        public async Task OnGetAsync()
        {
            CurrentStudentId = HttpContext.Session.GetInt32("CurrentStudentId");

            var students = await _context.Students
                                         .OrderBy(s => s.Email)
                                         .ToListAsync();
            StudentList = new SelectList(
                students,
                nameof(Student.StudentId),
                nameof(Student.Email),
                CurrentStudentId);

            if (CurrentStudentId.HasValue)
                CurrentStudent = await _context.Students.FindAsync(CurrentStudentId.Value);
        }
    }
}
