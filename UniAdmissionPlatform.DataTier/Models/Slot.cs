using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Slot
    {
        public Slot()
        {
            EventChecks = new HashSet<EventCheck>();
        }

        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int HighSchoolId { get; set; }
        public int Status { get; set; }

        public virtual HighSchool HighSchool { get; set; }
        public virtual ICollection<EventCheck> EventChecks { get; set; }
    }
}
