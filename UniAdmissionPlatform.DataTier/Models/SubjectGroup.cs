using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class SubjectGroup
    {
        public SubjectGroup()
        {
            SubjectGroupMajors = new HashSet<SubjectGroupMajor>();
            SubjectGroupSubjects = new HashSet<SubjectGroupSubject>();
            UniversityPrograms = new HashSet<UniversityProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubjectGroupMajor> SubjectGroupMajors { get; set; }
        public virtual ICollection<SubjectGroupSubject> SubjectGroupSubjects { get; set; }
        public virtual ICollection<UniversityProgram> UniversityPrograms { get; set; }
    }
}
