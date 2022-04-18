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
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int HighSchoolId { get; set; }

        public virtual HighSchool HighSchool { get; set; }
        public virtual ICollection<EventCheck> EventChecks { get; set; }
    }
}
