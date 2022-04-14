using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Province
    {
        public Province()
        {
            HighSchools = new HashSet<HighSchool>();
            Universities = new HashSet<University>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<HighSchool> HighSchools { get; set; }
        public virtual ICollection<University> Universities { get; set; }
    }
}
