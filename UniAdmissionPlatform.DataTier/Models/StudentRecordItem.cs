using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class StudentRecordItem
    {
        public int Id { get; set; }
        public float? Score { get; set; }
        public int SchoolRecordId { get; set; }
        public int SubjectId { get; set; }

        public virtual SchoolRecord SchoolRecord { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
