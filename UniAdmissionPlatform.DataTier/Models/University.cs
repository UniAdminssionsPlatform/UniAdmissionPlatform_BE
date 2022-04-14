using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class University
    {
        public University()
        {
            MajorDepartments = new HashSet<MajorDepartment>();
            UniversityEvents = new HashSet<UniversityEvent>();
            UniversityNews = new HashSet<UniversityNews>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public byte Description { get; set; }
        public int? ProvinceId { get; set; }
        public byte UniversityCode { get; set; }
        public byte WebsiteUrl { get; set; }
        public byte Email { get; set; }
        public int Status { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<MajorDepartment> MajorDepartments { get; set; }
        public virtual ICollection<UniversityEvent> UniversityEvents { get; set; }
        public virtual ICollection<UniversityNews> UniversityNews { get; set; }
    }
}
