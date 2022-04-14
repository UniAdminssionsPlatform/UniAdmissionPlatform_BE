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
        public byte Description { get; set; }

        public virtual MajorDepartment MajorDepartment { get; set; }
    }
}
