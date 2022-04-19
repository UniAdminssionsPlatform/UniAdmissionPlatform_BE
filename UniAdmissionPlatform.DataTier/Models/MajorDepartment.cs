using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class MajorDepartment
    {
        public MajorDepartment()
        {
            UniversityPrograms = new HashSet<UniversityProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MajorId { get; set; }
        public int UniversityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Major Major { get; set; }
        public virtual University University { get; set; }
        public virtual ICollection<UniversityProgram> UniversityPrograms { get; set; }
    }
}
