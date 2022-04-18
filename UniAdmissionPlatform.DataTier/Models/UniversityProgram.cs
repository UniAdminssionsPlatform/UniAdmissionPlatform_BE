using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class UniversityProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MajorDepartmentId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual MajorDepartment MajorDepartment { get; set; }
    }
}
