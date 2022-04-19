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
        public int HighSchoolId { get; set; }
        public int GenderId { get; set; }
        public int Status { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Religion { get; set; }
        public string IdCard { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual HighSchool HighSchool { get; set; }
        public virtual StudentCertification StudentCertification { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
        public virtual ICollection<SchoolRecord> SchoolRecords { get; set; }
    }
}
