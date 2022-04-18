using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Subject
    {
        public Subject()
        {
            StudentRecordItems = new HashSet<StudentRecordItem>();
            SubjectGroupSubjects = new HashSet<SubjectGroupSubject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudentRecordItem> StudentRecordItems { get; set; }
        public virtual ICollection<SubjectGroupSubject> SubjectGroupSubjects { get; set; }
    }
}
