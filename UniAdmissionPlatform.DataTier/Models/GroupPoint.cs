using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class GroupPoint
    {
        public GroupPoint()
        {
            UniversityPrograms = new HashSet<UniversityProgram>();
        }

        public int Id { get; set; }
        public double? Point { get; set; }

        public virtual ICollection<UniversityProgram> UniversityPrograms { get; set; }
    }
}
