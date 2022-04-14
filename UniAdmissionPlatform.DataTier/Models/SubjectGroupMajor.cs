using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class SubjectGroupMajor
    {
        public int SubjectGroupId { get; set; }
        public int MajorId { get; set; }

        public virtual Major Major { get; set; }
        public virtual SubjectGroup SubjectGroup { get; set; }
    }
}
