using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Student
    {
        public Student()
        {
            Participations = new HashSet<Participation>();
            SchoolRecords = new HashSet<SchoolRecord>();
        }

        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Account IdNavigation { get; set; }
        public virtual StudentCertification StudentCertification { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
        public virtual ICollection<SchoolRecord> SchoolRecords { get; set; }
    }
}
