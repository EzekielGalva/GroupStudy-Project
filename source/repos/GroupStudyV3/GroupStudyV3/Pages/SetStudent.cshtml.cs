using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GroupStudyV3.Pages
{
    public class SetStudentModel : PageModel
    {
        [BindProperty]
        public int? StudentId { get; set; }

        public IActionResult OnPost()
        {
            if (StudentId.HasValue)
            {
                HttpContext.Session.SetInt32("CurrentStudentId", StudentId.Value);
            }
            return RedirectToPage("/Index");
        }
    }
}
