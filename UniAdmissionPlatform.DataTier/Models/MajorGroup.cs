using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class MajorGroup
    {
        public MajorGroup()
        {
            Majors = new HashSet<Major>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }

        public virtual ICollection<Major> Majors { get; set; }
    }
}
