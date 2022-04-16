using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class SchoolRecord
    {
        public SchoolRecord()
        {
            StudentRecordItems = new HashSet<StudentRecordItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        public int SchoolYearId { get; set; }
        public float TotalScore { get; set; }

        public virtual SchoolYear SchoolYear { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<StudentRecordItem> StudentRecordItems { get; set; }
    }
}
