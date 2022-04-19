using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class SubjectGroupSubject
    {
        public int SubjectGroupId { get; set; }
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual SubjectGroup SubjectGroup { get; set; }
    }
}
