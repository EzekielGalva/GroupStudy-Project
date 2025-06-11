using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GroupStudyV3.Models;

    public class GroupStudyV3Context : DbContext
    {
        public GroupStudyV3Context (DbContextOptions<GroupStudyV3Context> options)
            : base(options)
        {
        }

        public DbSet<GroupStudyV3.Models.Student> Student { get; set; } = default!;

public DbSet<GroupStudyV3.Models.Assignment> Assignment { get; set; } = default!;

public DbSet<GroupStudyV3.Models.StudentAssignment> StudentAssignment { get; set; } = default!;

public DbSet<GroupStudyV3.Models.StudyGroup> StudyGroup { get; set; } = default!;

public DbSet<GroupStudyV3.Models.StudyGroupMember> StudyGroupMember { get; set; } = default!;
    }
