using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Student
    {
        public Student()
        {
            FollowEvents = new HashSet<FollowEvent>();
            Follows = new HashSet<Follow>();
            Participations = new HashSet<Participation>();
            SchoolRecords = new HashSet<SchoolRecord>();
            StudentCertifications = new HashSet<StudentCertification>();
        }

        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Account IdNavigation { get; set; }
        public virtual ICollection<FollowEvent> FollowEvents { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
        public virtual ICollection<SchoolRecord> SchoolRecords { get; set; }
        public virtual ICollection<StudentCertification> StudentCertifications { get; set; }
    }
}
