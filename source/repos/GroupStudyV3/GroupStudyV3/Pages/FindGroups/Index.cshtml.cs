using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

namespace GroupStudyV3.Pages.FindGroups
{
    public class IndexModel : PageModel
    {
        private readonly GroupStudyV2Context _ctx;
        public IndexModel(GroupStudyV2Context ctx) => _ctx = ctx;

        public int? CurrentStudentId { get; private set; }
        public Student? CurrentStudent { get; private set; }

        public SelectList Courses { get; set; } = default!;
        public SelectList AssignmentTypes { get; set; } = default!;
        public SelectList GenderList { get; set; } = default!;
        public SelectList StandingList { get; set; } = default!;
        public SelectList ExistingGroups { get; set; } = default!;

        [BindProperty] public int? CourseId { get; set; }
        [BindProperty] public string? TypeFilter { get; set; }
        [BindProperty] public string? GenderFilter { get; set; }
        [BindProperty] public string? StandingFilter { get; set; }
        [BindProperty] public List<int> SelectedStudentIds { get; set; } = new();
        [BindProperty] public string? GroupName { get; set; }
        [BindProperty] public int? ExistingGroupId { get; set; }
        [BindProperty] public string SubmitAction { get; set; } = "";

        public List<Student>? SearchResults { get; set; }

        public async Task OnGetAsync() => await PopulateListsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            await PopulateListsAsync();

            if (SubmitAction == "Search")
            {
                
                var q = _ctx.StudentAssignments
                            .Include(sa => sa.Student)
                            .Include(sa => sa.Assignment).ThenInclude(a => a.Course)
                            .AsQueryable();

                if (CourseId.HasValue)
                    q = q.Where(sa => sa.Assignment.CourseId == CourseId);
                if (!string.IsNullOrEmpty(TypeFilter))
                    q = q.Where(sa => sa.Assignment.Type == TypeFilter);
                if (!string.IsNullOrEmpty(GenderFilter))
                    q = q.Where(sa => sa.Student.Gender == GenderFilter);
                if (!string.IsNullOrEmpty(StandingFilter))
                    q = q.Where(sa => sa.Student.ClassStanding == StandingFilter);

                // Exclude current student from results
                CurrentStudentId = HttpContext.Session.GetInt32("CurrentStudentId");
                SearchResults = q
                    .Where(sa => sa.StudentId != CurrentStudentId)
                    .Select(sa => sa.Student)
                    .Distinct()
                    .ToList();

                return Page();
            }

            
            if (SubmitAction == "CreateGroup" && CurrentStudentId.HasValue)
            {
                // User must have an assignment in this course to create group
                var assignmentId = await _ctx.StudentAssignments
                    .Where(sa => sa.StudentId == CurrentStudentId.Value
                              && sa.Assignment.CourseId == CourseId)
                    .Select(sa => sa.AssignmentId)
                    .FirstOrDefaultAsync();

                if (assignmentId == 0)
                {
                    ModelState.AddModelError("", "You must have at least one assignment in that course to form a group.");
                    await PopulateListsAsync();
                    
                    return Page();
                }

                // Always include the creator in the group members
                if (!SelectedStudentIds.Contains(CurrentStudentId.Value))
                    SelectedStudentIds.Add(CurrentStudentId.Value);

                // Prevent empty groups
                if (!SelectedStudentIds.Any())
                {
                    ModelState.AddModelError("", "You must select at least one student (yourself or others) to create a group.");
                    return Page();
                }

                var name = string.IsNullOrWhiteSpace(GroupName)
                           ? $"Group_{assignmentId}_{DateTime.UtcNow:yyyyMMddHHmmss}"
                           : GroupName!;

                var group = new StudyGroup
                {
                    GroupName = name,
                    AssignmentId = assignmentId,
                    CreatedBy = CurrentStudentId.Value
                };
                _ctx.StudyGroups.Add(group);
                await _ctx.SaveChangesAsync();

                foreach (var sid in SelectedStudentIds.Distinct())
                {
                    _ctx.StudyGroupMembers.Add(new StudyGroupMember
                    {
                        StudyGroupId = group.StudyGroupId,
                        StudentId = sid
                    });
                }
                await _ctx.SaveChangesAsync();

                return RedirectToPage("/StudyGroups/Index");
            }

            // ADD TO EXISTING GROUP
            if (SubmitAction == "AddToExisting" && ExistingGroupId.HasValue)
            {
                foreach (var sid in SelectedStudentIds.Distinct())
                {
                    // Only add if not already in group
                    bool alreadyMember = await _ctx.StudyGroupMembers
                        .AnyAsync(m => m.StudyGroupId == ExistingGroupId && m.StudentId == sid);
                    if (!alreadyMember)
                    {
                        _ctx.StudyGroupMembers.Add(new StudyGroupMember
                        {
                            StudyGroupId = ExistingGroupId.Value,
                            StudentId = sid
                        });
                    }
                }
                await _ctx.SaveChangesAsync();

                return RedirectToPage("/StudyGroups/Index");
            }

            return Page();
        }

        private async Task PopulateListsAsync()
        {
            CurrentStudentId = HttpContext.Session.GetInt32("CurrentStudentId");
            if (CurrentStudentId.HasValue)
            {
                CurrentStudent = await _ctx.Students.FindAsync(CurrentStudentId.Value);
            }

            Courses = new SelectList(
                await _ctx.Courses.OrderBy(c => c.CourseCode).ToListAsync(),
                nameof(Course.CourseId),
                nameof(Course.CourseCode),
                CourseId);

            AssignmentTypes = new SelectList(
                new[] { "", "Homework", "Quiz", "Test", "Project" }
                    .Select(x => new { Value = x, Text = string.IsNullOrEmpty(x) ? "-- all types --" : x }),
                "Value", "Text", TypeFilter);

            GenderList = new SelectList(
                new[] { "", "Male", "Female" }
                    .Select(x => new { Value = x, Text = string.IsNullOrEmpty(x) ? "-- all genders --" : x }),
                "Value", "Text", GenderFilter);

            StandingList = new SelectList(
                new[] { "", "Freshman", "Sophomore", "Junior", "Senior" }
                    .Select(x => new { Value = x, Text = string.IsNullOrEmpty(x) ? "-- all standings --" : x }),
                "Value", "Text", StandingFilter);

            ExistingGroups = new SelectList(
                await _ctx.StudyGroups.OrderBy(g => g.GroupName).ToListAsync(),
                nameof(StudyGroup.StudyGroupId),
                nameof(StudyGroup.GroupName),
                ExistingGroupId);
        }
    }
}
