using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class District
    {
        public District()
        {
            Events = new HashSet<Event>();
            HighSchools = new HashSet<HighSchool>();
            Universities = new HashSet<University>();
            Wards = new HashSet<Ward>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<HighSchool> HighSchools { get; set; }
        public virtual ICollection<University> Universities { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
