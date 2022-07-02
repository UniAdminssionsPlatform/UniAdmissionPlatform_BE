using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Major
    {
        public Major()
        {
            MajorDepartmentMajorParents = new HashSet<MajorDepartment>();
            MajorDepartmentMajors = new HashSet<MajorDepartment>();
            NewsMajors = new HashSet<NewsMajor>();
            SubjectGroupMajors = new HashSet<SubjectGroupMajor>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int MajorGroupId { get; set; }

        public virtual MajorGroup MajorGroup { get; set; }
        public virtual ICollection<MajorDepartment> MajorDepartmentMajorParents { get; set; }
        public virtual ICollection<MajorDepartment> MajorDepartmentMajors { get; set; }
        public virtual ICollection<NewsMajor> NewsMajors { get; set; }
        public virtual ICollection<SubjectGroupMajor> SubjectGroupMajors { get; set; }
    }
}
