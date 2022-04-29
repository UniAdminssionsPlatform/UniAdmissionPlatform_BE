using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Province
    {
        public Province()
        {
            Districts = new HashSet<District>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
